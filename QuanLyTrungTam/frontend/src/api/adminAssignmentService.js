import apiClient from './BaseApi';

const ensureSuccess = (response, fallbackMessage) => {
  const isError = response?.success === false || response?.Success === false;

  if (isError) {
    const error = new Error(response.message || response.Message || fallbackMessage);
    error.status = response.status;
    error.response = response.raw || response;
    throw error;
  }

  return response;
};

const unwrapData = (response) => {
  if (response?.Success === true) {
    return response.Data;
  }

  if (response?.success === true) {
    return response.data;
  }

  return response;
};

const buildQueryString = (params = {}) => {
  const searchParams = new URLSearchParams();

  Object.entries(params).forEach(([key, value]) => {
    if (value === undefined || value === null || value === '') {
      return;
    }

    searchParams.set(key, value);
  });

  const queryString = searchParams.toString();
  return queryString ? `?${queryString}` : '';
};

const adminAssignmentService = {
  getBootstrap: async () => {
    const response = ensureSuccess(
      await apiClient.get('/AdminAssignment/bootstrap'),
      'Khong the tai du lieu khoi tao bai tap.'
    );

    return unwrapData(response);
  },

  getAssignments: async (filters = {}) => {
    const response = ensureSuccess(
      await apiClient.get(`/AdminAssignment/assignments${buildQueryString(filters)}`),
      'Khong the tai danh sach bai tap/de thi.'
    );

    return unwrapData(response);
  },

  getAssignmentDetail: async (assignmentId) => {
    const response = ensureSuccess(
      await apiClient.get(`/AdminAssignment/assignments/${assignmentId}`),
      'Khong the tai chi tiet bai tap/de thi.'
    );

    return unwrapData(response);
  },

  createAssignment: async (payload) => {
    const response = ensureSuccess(
      await apiClient.post('/AdminAssignment/assignments', payload),
      'Khong the tao bai tap/de thi.'
    );

    return unwrapData(response);
  },

  updateAssignment: async (assignmentId, payload) => {
    const response = ensureSuccess(
      await apiClient.put(`/AdminAssignment/assignments/${assignmentId}`, payload),
      'Khong the cap nhat bai tap/de thi.'
    );

    return unwrapData(response);
  },

  deleteAssignment: async (assignmentId) => {
    const response = ensureSuccess(
      await apiClient.delete(`/AdminAssignment/assignments/${assignmentId}`),
      'Khong the xoa bai tap/de thi.'
    );

    return unwrapData(response);
  },

  getQuestionBank: async (filters = {}) => {
    const response = ensureSuccess(
      await apiClient.get(`/AdminAssignment/question-bank${buildQueryString(filters)}`),
      'Khong the tai ngan hang cau hoi.'
    );

    return unwrapData(response);
  },

  getQuestionDetail: async (questionId) => {
    const response = ensureSuccess(
      await apiClient.get(`/AdminAssignment/question-bank/${questionId}`),
      'Khong the tai chi tiet cau hoi.'
    );

    return unwrapData(response);
  },

  createQuestion: async (payload) => {
    const response = ensureSuccess(
      await apiClient.post('/AdminAssignment/question-bank', payload),
      'Khong the tao cau hoi.'
    );

    return unwrapData(response);
  },

  updateQuestion: async (questionId, payload) => {
    const response = ensureSuccess(
      await apiClient.put(`/AdminAssignment/question-bank/${questionId}`, payload),
      'Khong the cap nhat cau hoi.'
    );

    return unwrapData(response);
  },

  deleteQuestion: async (questionId) => {
    const response = ensureSuccess(
      await apiClient.delete(`/AdminAssignment/question-bank/${questionId}`),
      'Khong the xoa cau hoi.'
    );

    return unwrapData(response);
  },

  importQuestions: async (payload) => {
    const response = ensureSuccess(
      await apiClient.post('/AdminAssignment/question-bank/import', payload),
      'Khong the import ngan hang cau hoi.'
    );

    return unwrapData(response);
  },
};

export default adminAssignmentService;
