import api from './api';

export const attendanceService = {
  getAll: async () => {
    const response = await api.get('/attendance');
    return response.data;
  },

  getStudentAttendance: async (studentId) => {
    const response = await api.get(`/attendance/student/${studentId}`);
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/attendance/${id}`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/attendance', data);
    return response.data;
  },

  update: async (id, data) => {
    await api.put(`/attendance/${id}`, data);
  },

  delete: async (id) => {
    await api.delete(`/attendance/${id}`);
  },
};



