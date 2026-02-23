import api from './api';

export const gatePassService = {
  getAll: async () => {
    const response = await api.get('/gatepass');
    return response.data;
  },

  getStudentRequests: async (studentId) => {
    const response = await api.get(`/gatepass/student/${studentId}`);
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/gatepass/${id}`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/gatepass', data);
    return response.data;
  },

  approve: async (id, status) => {
    await api.put(`/gatepass/${id}/approve`, { status });
  },

  delete: async (id) => {
    await api.delete(`/gatepass/${id}`);
  },
};



