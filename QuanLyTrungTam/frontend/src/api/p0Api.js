import apiClient from "./BaseApi";

const unwrap = (result) => {
  if (result?.success === false) {
    throw new Error(result.message || "API request failed");
  }
  return result?.data ?? result;
};

export const p0Api = {
  users: {
    list: async () => unwrap(await apiClient.get("users")),
    get: async (id) => unwrap(await apiClient.get(`users/${id}`)),
    create: async (payload) => unwrap(await apiClient.post("users", payload)),
    update: async (id, payload) => unwrap(await apiClient.put(`users/${id}`, payload)),
    remove: async (id) => unwrap(await apiClient.delete(`users/${id}`)),
    updateRoles: async (id, roleIds) => unwrap(await apiClient.put(`users/${id}/roles`, { roleIds })),
  },
  roles: {
    list: async () => {
      const matrix = await apiClient.get("Permissions/matrix");
      return matrix?.roles ?? [];
    },
  },
  courses: {
    list: async () => unwrap(await apiClient.get("courses")),
    create: async (payload) => unwrap(await apiClient.post("courses", payload)),
    update: async (id, payload) => unwrap(await apiClient.put(`courses/${id}`, payload)),
    remove: async (id) => unwrap(await apiClient.delete(`courses/${id}`)),
  },
  classes: {
    list: async () => unwrap(await apiClient.get("classes")),
    create: async (payload) => unwrap(await apiClient.post("classes", payload)),
    update: async (id, payload) => unwrap(await apiClient.put(`classes/${id}`, payload)),
    remove: async (id) => unwrap(await apiClient.delete(`classes/${id}`)),
    students: async (classId) => unwrap(await apiClient.get(`classes/${classId}/students`)),
    addStudent: async (classId, studentId) => unwrap(await apiClient.post(`classes/${classId}/students`, { studentId })),
    removeStudent: async (classId, studentId) => unwrap(await apiClient.delete(`classes/${classId}/students/${studentId}`)),
    assignedToMe: async () => unwrap(await apiClient.get("classes/assigned-to-me")),
  },
  attendance: {
    get: async (classId, sessionId) => unwrap(await apiClient.get(`classes/${classId}/sessions/${sessionId}/attendance`)),
    save: async (classId, sessionId, items) => unwrap(await apiClient.post(`classes/${classId}/sessions/${sessionId}/attendance`, { items })),
  },
  parents: {
    children: async (parentUserId) => unwrap(await apiClient.get(`parents/${parentUserId}/children`)),
    addChild: async (parentUserId, studentId) => unwrap(await apiClient.post(`parents/${parentUserId}/children`, { studentId })),
    removeChild: async (parentUserId, studentId) => unwrap(await apiClient.delete(`parents/${parentUserId}/children/${studentId}`)),
    attendance: async (parentUserId) => unwrap(await apiClient.get(`parents/${parentUserId}/children/attendance`)),
  },
  students: {
    attendance: async () => unwrap(await apiClient.get("students/me/attendance")),
  },
  schedule: {
    teacherBoard: async () => apiClient.get("Schedule/teacher-board"),
    create: async (payload) => unwrap(await apiClient.post("Schedule", payload)),
  },
};
