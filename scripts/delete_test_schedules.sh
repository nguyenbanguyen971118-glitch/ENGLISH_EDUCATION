#!/usr/bin/env bash
set -euo pipefail

BASE='http://127.0.0.1:5050/api'

login_json=$(curl -sS -X POST "$BASE/Auth/login" \
  -H 'Content-Type: application/json' \
  -d '{"data":{"email":"admin@qltt.local","password":"QlttAdmin@2026!A9"}}')

token=$(node -e 'const fs=require("fs"); const j=JSON.parse(fs.readFileSync(0,"utf8")); process.stdout.write(j.data.accessToken||j.data.token||"")' <<<"$login_json")

if [[ -z "$token" ]]; then
  echo "Failed to obtain token" >&2
  exit 1
fi

ids=(
  "bf3b0488-61af-4665-ac1a-631a3f84b096"
  "f213f280-b6ce-4746-900a-ce7515019b8f"
  "3b427e85-d7d7-47fc-a226-7da4d13d7491"
)

for id in "${ids[@]}"; do
  resp=$(curl -sS -i -X DELETE "$BASE/Schedule/$id" -H "Authorization: Bearer $token")
  status=$(printf '%s' "$resp" | head -n 1)
  body=$(printf '%s' "$resp" | sed -n '/^$/,$p' | tail -n +2)
  echo "DELETE $id -> $status"
  if [[ -n "$body" ]]; then
    echo "$body"
  fi
done

board_json=$(curl -sS "$BASE/Schedule/admin-board?weekStart=2026-05-05T00:00:00" -H "Authorization: Bearer $token")
node -e 'const fs=require("fs"); const board=JSON.parse(fs.readFileSync(0,"utf8")); const d=board.data||board; const list=d.schedules||[]; const testRows=list.filter(s=>String(s.subject||s.Subject||"").includes("Test multi-period")); console.log("REMAINING_TEST_ROWS", JSON.stringify(testRows)); console.log("TOTAL_COUNT", list.length);' <<<"$board_json"
