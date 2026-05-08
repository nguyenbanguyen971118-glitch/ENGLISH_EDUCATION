const base = 'http://127.0.0.1:5050/api';

const ids = [
  'bf3b0488-61af-4665-ac1a-631a3f84b096',
  'f213f280-b6ce-4746-900a-ce7515019b8f',
  '3b427e85-d7d7-47fc-a226-7da4d13d7491'
];

async function main() {
  const loginRes = await fetch(`${base}/Auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ data: { email: 'admin@qltt.local', password: 'QlttAdmin@2026!A9' } })
  });
  const loginJson = await loginRes.json();
  if (!loginRes.ok) {
    throw new Error(`login failed: ${JSON.stringify(loginJson)}`);
  }

  const token = loginJson?.data?.accessToken || loginJson?.data?.token;
  if (!token) {
    throw new Error('missing access token');
  }

  for (const id of ids) {
    const res = await fetch(`${base}/Schedule/${id}`, {
      method: 'DELETE',
      headers: { Authorization: `Bearer ${token}` }
    });
    const text = await res.text();
    console.log('DELETE', id, res.status, text);
  }

  const boardRes = await fetch(`${base}/Schedule/admin-board?weekStart=2026-05-05T00:00:00`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  const boardJson = await boardRes.json();
  const list = boardJson?.data?.schedules || boardJson?.schedules || [];
  const testRows = list.filter((schedule) => String(schedule.subject || schedule.Subject || '').includes('Test multi-period'));
  console.log('REMAINING_TEST_ROWS', JSON.stringify(testRows));
  console.log('TOTAL_COUNT', list.length);
}

main().catch((error) => {
  console.error('SCRIPT_ERR', error?.stack || error);
  process.exit(1);
});
