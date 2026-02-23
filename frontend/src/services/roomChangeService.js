import api from './api';

export const roomChangeService = {
  getAll: async () => {
    const response = await api.get('/roomchange');
    return response.data;
  },

  getStudentRequests: async (studentId) => {
    const response = await api.get(`/roomchange/student/${studentId}`);
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/roomchange/${id}`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/roomchange', data);
    return response.data;
  },

  approve: async (id, status) => {
    await api.put(`/roomchange/${id}/approve`, { status });
  },

  delete: async (id) => {
    await api.delete(`/roomchange/${id}`);
  },
};



