import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Plus, Trash2 } from "lucide-react";
import { p0Api } from "../../api/p0Api";
import { CLASS_SHIFT_OPTIONS, SHIFT_TO_PERIODS } from "../../constants/scheduleTime";

const dayToJsDay = {
  "Thu 2": 1,
  "Thu 3": 2,
  "Thu 4": 3,
  "Thu 5": 4,
  "Thu 6": 5,
  "Thu 7": 6,
  "Chu nhat": 0,
};

const parseLocalDate = (value) => {
  if (!value) return new Date();
  const [year, month, day] = value.split("-").map(Number);
  return new Date(year, month - 1, day);
};

const formatLocalDate = (date) => {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};

const getFirstDateForDay = (startDate, day) => {
  const targetDay = dayToJsDay[day];
  const date = parseLocalDate(startDate);
  const diff = (targetDay - date.getDay() + 7) % 7;
  date.setDate(date.getDate() + diff);
  return date;
};

export default function AdminCreateClass() {
  const navigate = useNavigate();
  const [courses, setCourses] = useState([]);
  const [teachers, setTeachers] = useState([]);
  const [form, setForm] = useState({
    name: "",
    courseId: "",
    teacherId: "",
    status: "active",
    capacity: "",
    startDate: "",
    endDate: "",
    schedules: [],
    description: "",
  });
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [toast, setToast] = useState("");

  useEffect(() => {
    Promise.all([p0Api.courses.list(), p0Api.users.list()])
      .then(([courseRows, userRows]) => {
        setCourses(courseRows || []);
        setTeachers((userRows || []).filter((item) => (item.roles || []).some((role) => role.name === "Giao_Vien") && item.profileId));
      })
      .catch((err) => setError(err.message || "Khong tai duoc du lieu tao lop."));
  }, []);

  const addSchedule = () => setForm({ ...form, schedules: [...form.schedules, { id: Date.now(), day: "", shift: "" }] });
  const deleteSchedule = (id) => setForm({ ...form, schedules: form.schedules.filter((s) => s.id !== id) });
  const updateSchedule = (id, field, value) => {
    setForm({ ...form, schedules: form.schedules.map((s) => (s.id === id ? { ...s, [field]: value } : s)) });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    if (!form.name.trim() || !form.courseId || !form.teacherId) {
      setError("Vui long nhap ten lop, khoa hoc va giao vien.");
      return;
    }

    const scheduleRows = form.schedules.filter((item) => item.day || item.shift);
    if (scheduleRows.some((item) => !item.day || !item.shift)) {
      setError("Vui long chon day du thu va ca cho tung lich hoc.");
      return;
    }

    setSaving(true);
    try {
      const createdClass = await p0Api.classes.create({
        name: form.name.trim(),
        startDate: form.startDate || null,
        endDate: form.endDate || null,
        capacity: form.capacity === "" ? null : Number(form.capacity),
        active: form.status === "active",
        courseIds: [form.courseId],
        teacherIds: [form.teacherId],
      });

      const classId = createdClass?.id;
      if (!classId) {
        throw new Error("Khong lay duoc ma lop vua tao.");
      }

      const schedulePayloads = scheduleRows.map((schedule) => {
        const periods = SHIFT_TO_PERIODS[schedule.shift];
        const sessionDate = getFirstDateForDay(form.startDate, schedule.day);
        if (form.endDate && sessionDate > parseLocalDate(form.endDate)) {
          throw new Error(`Lich ${schedule.day} nam ngoai khoang ngay bat dau/ket thuc.`);
        }

        return {
          MaLopHoc: classId,
          MaPhongHoc: null,
          NgayHoc: formatLocalDate(sessionDate),
          MaTietBatDau: periods.start,
          MaTietKetThuc: periods.end,
          TieuDe: form.name.trim(),
          NoiDung: `Lich hoc ${schedule.day} - ${schedule.shift}`,
        };
      });

      for (const payload of schedulePayloads) {
        await p0Api.schedule.create(payload);
      }

      setToast(schedulePayloads.length > 0 ? "Da tao lop hoc va lich hoc." : "Da tao lop hoc.");
      setTimeout(() => navigate("/admin/classes"), 700);
    } catch (err) {
      setError(err.message || "Tao lop hoc that bai.");
    } finally {
      setSaving(false);
    }
  };

  const daysOfWeek = ["Thu 2", "Thu 3", "Thu 4", "Thu 5", "Thu 6", "Thu 7", "Chu nhat"];
  const shifts = CLASS_SHIFT_OPTIONS.map((item) => item.label);

  return (
    <div className="container-fluid p-4">
      <div className="mb-3">
        <button className="btn btn-link text-decoration-none" onClick={() => navigate(-1)}>Quay lai</button>
      </div>
      <h3 className="mb-4">Tao lop hoc moi</h3>
      {toast && <div className="alert alert-success">{toast}</div>}
      {error && <div className="alert alert-danger">{error}</div>}

      <div className="row">
        <div className="col-md-6">
          <div className="card p-4 shadow-sm">
            <h5 className="mb-3">Thong tin lop hoc</h5>
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label className="form-label fw-bold">Ten lop hoc *</label>
                <input className="form-control" value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} />
              </div>

              <div className="row">
                <div className="col-md-6 mb-3">
                  <label className="form-label fw-bold">Khoa hoc *</label>
                  <select className="form-select" value={form.courseId} onChange={(e) => setForm({ ...form, courseId: e.target.value })}>
                    <option value="">Chon khoa hoc</option>
                    {courses.map((course) => <option key={course.id} value={course.id}>{course.name}</option>)}
                  </select>
                </div>
                <div className="col-md-6 mb-3">
                  <label className="form-label fw-bold">Trang thai *</label>
                  <select className="form-select" value={form.status} onChange={(e) => setForm({ ...form, status: e.target.value })}>
                    <option value="active">Dang hoat dong</option>
                    <option value="inactive">Tam dung</option>
                  </select>
                </div>
              </div>

              <div className="mb-3">
                <label className="form-label fw-bold">Giao vien *</label>
                <select className="form-select" value={form.teacherId} onChange={(e) => setForm({ ...form, teacherId: e.target.value })}>
                  <option value="">Chon giao vien</option>
                  {teachers.map((teacher) => <option key={teacher.profileId} value={teacher.profileId}>{teacher.fullName}</option>)}
                </select>
              </div>

              <div className="row">
                <div className="col-md-4 mb-3">
                  <label className="form-label fw-bold">Suc chua</label>
                  <input type="number" className="form-control" value={form.capacity} onChange={(e) => setForm({ ...form, capacity: e.target.value })} />
                </div>
                <div className="col-md-4 mb-3">
                  <label className="form-label fw-bold">Ngay bat dau</label>
                  <input type="date" className="form-control" value={form.startDate} onChange={(e) => setForm({ ...form, startDate: e.target.value })} />
                </div>
                <div className="col-md-4 mb-3">
                  <label className="form-label fw-bold">Ngay ket thuc</label>
                  <input type="date" className="form-control" value={form.endDate} onChange={(e) => setForm({ ...form, endDate: e.target.value })} />
                </div>
              </div>

              <div className="mb-4">
                <div className="d-flex align-items-center justify-content-between mb-2">
                  <label className="form-label fw-bold mb-0">Lich hoc</label>
                  <button type="button" className="btn btn-outline-primary btn-sm d-flex align-items-center gap-1" onClick={addSchedule}><Plus size={16} /> Them lich</button>
                </div>
                {form.schedules.length > 0 ? (
                  <div className="d-flex flex-column gap-2">
                    {form.schedules.map((schedule) => (
                      <div key={schedule.id} className="d-flex align-items-center gap-2 p-2 border rounded bg-light">
                        <select className="form-select form-select-sm" value={schedule.day} onChange={(e) => updateSchedule(schedule.id, "day", e.target.value)}>
                          <option value="">Chon thu</option>
                          {daysOfWeek.map((day) => <option key={day} value={day}>{day}</option>)}
                        </select>
                        <select className="form-select form-select-sm" value={schedule.shift} onChange={(e) => updateSchedule(schedule.id, "shift", e.target.value)}>
                          <option value="">Chon ca</option>
                          {shifts.map((shift) => <option key={shift} value={shift}>{shift}</option>)}
                        </select>
                        <button type="button" className="btn btn-link text-danger p-1" onClick={() => deleteSchedule(schedule.id)}><Trash2 size={18} /></button>
                      </div>
                    ))}
                  </div>
                ) : <small className="text-muted d-block mt-1">Chua co lich hoc nao duoc them.</small>}
                <small className="text-muted">Lich hoc hien chi hien thi trong form; API P0 luu thong tin lop/khoa/giao vien.</small>
              </div>

              <div className="d-flex justify-content-end gap-2">
                <button type="button" className="btn btn-light" onClick={() => navigate(-1)}>Huy</button>
                <button type="submit" className="btn btn-primary" disabled={saving}>{saving ? "Dang tao..." : "Tao lop hoc"}</button>
              </div>
            </form>
          </div>
        </div>

        <div className="col-md-6 d-flex nalign-items-center justify-content-center">
          <div className="text-center">
            <div style={{ fontSize: "70px" }}><i className="bi bi-mortarboard"></i></div>
            <h5 className="mt-3">Tao lop hoc moi</h5>
            <p className="text-muted">Them lop hoc vao he thong de quan ly hoc vien va lich hoc.</p>
          </div>
        </div>
      </div>
    </div>
  );
}
