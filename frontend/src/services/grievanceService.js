import api from './api';

export const grievanceService = {
  getAll: async () => {
    const response = await api.get('/grievance');
    return response.data;
  },

  getStudentGrievances: async (studentId) => {
    const response = await api.get(`/grievance/student/${studentId}`);
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/grievance/${id}`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/grievance', data);
    return response.data;
  },

  resolve: async (id, status) => {
    await api.put(`/grievance/${id}/resolve`, { status });
  },

  delete: async (id) => {
    await api.delete(`/grievance/${id}`);
  },
};



