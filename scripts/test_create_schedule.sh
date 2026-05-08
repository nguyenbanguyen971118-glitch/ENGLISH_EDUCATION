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

echo "LOGIN_OK"

board_json=$(curl -sS "$BASE/Schedule/admin-board?weekStart=2026-05-05T00:00:00" -H "Authorization: Bearer $token")
create_json=$(node -e 'const fs=require("fs"); const board=JSON.parse(fs.readFileSync(0,"utf8")); const d=board.data||board; const classes=d.classes||[]; const schedules=d.schedules||[]; const targetDate="2026-05-08"; const occupiedSlots=new Set(schedules.filter(s=>String(s.ngayHoc||s.NgayHoc||"").startsWith(targetDate)).map(s=>Number(s.slotId||s.SlotId))); const freeSlot=[1,2,3,4,5,6,7,8,9,10,11,12].find(slot=>!occupiedSlots.has(slot))||1; const firstClass=classes[0]; if(!firstClass) throw new Error("No classes available"); const payload={MaLopHoc:firstClass.id||firstClass.Id,MaPhongHoc:null,NgayHoc:"2026-05-08T00:00:00",MaTietBatDau:freeSlot,MaTietKetThuc:freeSlot+1<=12?freeSlot+1:freeSlot,TieuDe:"Test multi-period",NoiDung:""}; process.stdout.write(JSON.stringify(payload));' <<<"$board_json")
printf '%s' "$create_json" > /tmp/qltt_create.json

create_resp=$(curl -sS -i -X POST "$BASE/Schedule" \
  -H 'Content-Type: application/json' \
  -H "Authorization: Bearer $token" \
  --data-binary "$create_json")

echo "$create_resp"

create_id=$(printf '%s' "$create_resp" | node -e 'const fs=require("fs"); const input=fs.readFileSync(0,"utf8"); const body=input.split(/\r?\n\r?\n/).slice(1).join("\n\n"); const j=JSON.parse(body); process.stdout.write(j.id||j.Id||j.data?.id||"");')
printf '%s' "$create_id" > /tmp/qltt_created_id.txt

node -e 'const fs=require("fs"); const board=JSON.parse(fs.readFileSync(0,"utf8")); const d=board.data||board; const list=d.schedules||[]; console.log("BOARD_COUNT", list.length); const targetClass=JSON.parse(fs.readFileSync("/tmp/qltt_create.json","utf8")).MaLopHoc; const matched=list.filter(s=>String(s.maLopHoc||s.MaLopHoc)===String(targetClass)&&String(s.ngayHoc||s.NgayHoc||"").startsWith("2026-05-08")); console.log("MATCHED", JSON.stringify(matched));' <<<"$board_json"
node -e 'const fs=require("fs"); const board=JSON.parse(fs.readFileSync(0,"utf8")); const d=board.data||board; const list=d.schedules||[]; const createdId=fs.readFileSync("/tmp/qltt_created_id.txt","utf8").trim(); const matched=list.filter(s=>String(s.id||s.Id||s.maBuoiHoc||s.MaBuoiHoc)===createdId); console.log("MATCHED_BY_ID", JSON.stringify(matched));' <<<"$board_json"
node -e 'const fs=require("fs"); const board=JSON.parse(fs.readFileSync(0,"utf8")); const d=board.data||board; const list=d.schedules||[]; const rows=list.filter(s=>String(s.ngayHoc||s.NgayHoc||"").startsWith("2026-05-08")); console.log("ROWS_2026_05_08", JSON.stringify(rows));' <<<"$board_json"
