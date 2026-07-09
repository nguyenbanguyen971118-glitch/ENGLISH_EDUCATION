const resolveMock = (payload) => Promise.resolve(payload);

const logMock = (endpoint, payload) => {
  console.log(`[mock-service] ${endpoint}`, payload);
};

export const adminExerciseExamManagementService = {
  getExerciseExams() {
    return resolveMock([]);
  },
  assignExerciseExam(payload) {
    logMock('POST /api/exercise-exams/assign', payload);
    return resolveMock({ success: true });
  },
  getQuestionBanks() {
    return resolveMock([]);
  },
  createQuestionBank(payload) {
    logMock('POST /api/question-banks', payload);
    return resolveMock({ success: true });
  },
  updateQuestionBank(id, payload) {
    logMock(`PUT /api/question-banks/${id}`, payload);
    return resolveMock({ success: true });
  },
  deleteQuestionBank(id) {
    logMock(`DELETE /api/question-banks/${id}`, { id });
    return resolveMock({ success: true });
  },
  getQuestionBankExercises(id) {
    return resolveMock([]);
  },
  createQuestionBankExercise(id, payload) {
    logMock(`POST /api/question-banks/${id}/exercises`, payload);
    return resolveMock({ success: true });
  },
  updateQuestionBankExercise(id, exerciseId, payload) {
    logMock(`PUT /api/question-banks/${id}/exercises/${exerciseId}`, payload);
    return resolveMock({ success: true });
  },
  deleteQuestionBankExercise(id, exerciseId) {
    logMock(`DELETE /api/question-banks/${id}/exercises/${exerciseId}`, { id, exerciseId });
    return resolveMock({ success: true });
  },
  approveQuestionBankExercise(id, exerciseId) {
    logMock(`PUT /api/question-banks/${id}/exercises/${exerciseId}/approve`, { id, exerciseId });
    return resolveMock({ success: true });
  },
  deactivateQuestionBankExercise(id, exerciseId) {
    logMock(`PUT /api/question-banks/${id}/exercises/${exerciseId}/deactivate`, { id, exerciseId });
    return resolveMock({ success: true });
  },
  createExamTemplate(id, payload) {
    logMock(`POST /api/question-banks/${id}/exam-templates`, payload);
    return resolveMock({ success: true });
  },
  saveSourcePool(id, payload) {
    logMock(`POST /api/question-banks/${id}/source-pool`, payload);
    return resolveMock({ success: true });
  },
  generateExams(id, payload) {
    logMock(`POST /api/question-banks/${id}/generate-exams`, payload);
    return resolveMock({ success: true });
  },
};

export default adminExerciseExamManagementService;
