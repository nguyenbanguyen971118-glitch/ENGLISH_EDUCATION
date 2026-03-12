import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function AdminCreateClass() {

  const navigate = useNavigate();

  const [form,setForm] = useState({
    name:"",
    level:"",
    teacher:"",
    status:"active",
    days:[],
    startTime:"",
    endTime:"",
    description:""
  });

  const daysOfWeek = [
    "Thứ 2","Thứ 3","Thứ 4","Thứ 5","Thứ 6","Thứ 7","Chủ nhật"
  ];

  const toggleDay = (day)=>{
    if(form.days.includes(day)){
      setForm({...form,days:form.days.filter(d=>d!==day)})
    }else{
      setForm({...form,days:[...form.days,day]})
    }
  }

  const handleSubmit=(e)=>{
    e.preventDefault();
    console.log(form);
  }

  return (

<div className="container-fluid p-4">

{/* BACK */}
<div className="mb-3">
<button
className="btn btn-link"
onClick={()=>navigate(-1)}
>
← Quay lại
</button>
</div>

<h3 className="mb-4">
📘 Tạo lớp học mới
</h3>

<div className="row">

{/* FORM */}
<div className="col-md-6">

<div className="card p-4">

<h5 className="mb-3">
Thông tin lớp học
</h5>

<form onSubmit={handleSubmit}>

{/* NAME */}
<div className="mb-3">
<label className="form-label">
Tên lớp học *
</label>

<input
className="form-control"
placeholder="Ví dụ: Lớp tiếng Anh cơ bản"
onChange={(e)=>setForm({...form,name:e.target.value})}
/>
</div>

{/* LEVEL */}
<div className="mb-3">
<label className="form-label">
Cấp độ *
</label>

<select
className="form-select"
onChange={(e)=>setForm({...form,level:e.target.value})}
>
<option>Chọn trình độ</option>
<option>A1</option>
<option>A2</option>
<option>B1</option>
<option>B2</option>
</select>
</div>

{/* TEACHER */}
<div className="mb-3">
<label className="form-label">
Giáo viên *
</label>

<select
className="form-select"
onChange={(e)=>setForm({...form,teacher:e.target.value})}
>
<option>Chọn giáo viên</option>
<option>Giáo viên Lan</option>
<option>Giáo viên Minh</option>
</select>

</div>

{/* STATUS */}
<div className="mb-3">
<label className="form-label">
Trạng thái *
</label>

<select
className="form-select"
onChange={(e)=>setForm({...form,status:e.target.value})}
>
<option value="active">Đang hoạt động</option>
<option value="inactive">Tạm dừng</option>
</select>

</div>

{/* DAYS */}
<div className="mb-3">

<label className="form-label">
Ngày học trong tuần *
</label>

<div className="d-flex flex-wrap gap-2">

{daysOfWeek.map(day=>(

<button
type="button"
key={day}
className={`btn ${
form.days.includes(day)
? "btn-primary"
: "btn-outline-secondary"
}`}
onClick={()=>toggleDay(day)}
>
{day}
</button>

))}

</div>

</div>

{/* TIME */}
<div className="row mb-3">

<div className="col">

<label className="form-label">
Giờ bắt đầu *
</label>

<input
type="time"
className="form-control"
onChange={(e)=>setForm({...form,startTime:e.target.value})}
/>

</div>

<div className="col">

<label className="form-label">
Giờ kết thúc *
</label>

<input
type="time"
className="form-control"
onChange={(e)=>setForm({...form,endTime:e.target.value})}
/>

</div>

</div>

{/* DESC */}

<div className="mb-3">

<label className="form-label">
Mô tả
</label>

<textarea
className="form-control"
rows="3"
placeholder="Nhập mô tả lớp học..."
onChange={(e)=>setForm({...form,description:e.target.value})}
/>

</div>

<div className="d-flex justify-content-end gap-2">

<button
type="button"
className="btn btn-light"
onClick={()=>navigate(-1)}
>
Hủy
</button>

<button
type="submit"
className="btn btn-primary"
>
📘 Tạo lớp học
</button>

</div>

</form>

</div>

</div>

{/* RIGHT SIDE */}
<div className="col-md-6 d-flex align-items-center justify-content-center">

<div className="text-center">

<div style={{fontSize:"70px"}}>
🎓
</div>

<h5 className="mt-3">
Tạo lớp học mới
</h5>

<p className="text-muted">
Thêm lớp học vào hệ thống để quản lý học viên và lịch học.
</p>

</div>

</div>

</div>

</div>
);
}