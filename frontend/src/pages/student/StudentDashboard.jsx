import { useState, useEffect } from 'react';
import DashboardLayout from '../../components/DashboardLayout';
import DataTable from '../../components/DataTable';
import Modal from '../../components/Modal';
import { useAuth } from '../../context/AuthContext';
import { attendanceService } from '../../services/attendanceService';
import { gatePassService } from '../../services/gatePassService';
import { roomChangeService } from '../../services/roomChangeService';
import { grievanceService } from '../../services/grievanceService';
import { studentService } from '../../services/studentService';
import { roomService } from '../../services/roomService';

const StudentDashboard = () => {
  const { user } = useAuth();
  const [activeTab, setActiveTab] = useState('attendance');
  const [student, setStudent] = useState(null);
  const [attendances, setAttendances] = useState([]);
  const [gatePasses, setGatePasses] = useState([]);
  const [roomChanges, setRoomChanges] = useState([]);
  const [grievances, setGrievances] = useState([]);
  const [rooms, setRooms] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [formData, setFormData] = useState({});

  const menuItems = [
    { path: '/student/dashboard', label: 'Dashboard', icon: '🏠' },
    { path: '/student/attendance', label: 'Attendance', icon: '📊' },
    { path: '/student/gatepass', label: 'Gate Pass', icon: '🚪' },
    { path: '/student/roomchange', label: 'Room Change', icon: '🏠' },
    { path: '/student/grievances', label: 'Grievances', icon: '💬' },
  ];

  useEffect(() => {
    loadStudentData();
    loadData();
  }, [activeTab]);

  const loadStudentData = async () => {
    try {
      const students = await studentService.getAll();
      const currentStudent = students.find(s => s.userId === user?.id);
      if (currentStudent) {
        setStudent(currentStudent);
      }
    } catch (error) {
      console.error('Error loading student data:', error);
    }
  };

  const loadData = async () => {
    if (!student) return;

    try {
      switch (activeTab) {
        case 'attendance':
          setAttendances(await attendanceService.getStudentAttendance(student.studentId));
          break;
        case 'gatepass':
          setGatePasses(await gatePassService.getStudentRequests(student.studentId));
          break;
        case 'roomchange':
          setRoomChanges(await roomChangeService.getStudentRequests(student.studentId));
          setRooms(await roomService.getAll());
          break;
        case 'grievances':
          setGrievances(await grievanceService.getStudentGrievances(student.studentId));
          break;
      }
    } catch (error) {
      console.error('Error loading data:', error);
      alert('Error loading data');
    }
  };

  const handleCreate = () => {
    setFormData({});
    setIsModalOpen(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!student) return;

    try {
      switch (activeTab) {
        case 'gatepass':
          await gatePassService.create({
            ...formData,
            requestDate: new Date().toISOString(),
          });
          break;
        case 'roomchange':
          await roomChangeService.create({
            ...formData,
            currentRoomId: student.roomNumber ? rooms.find(r => r.roomNumber === student.roomNumber)?.roomId : 0,
          });
          break;
        case 'grievances':
          await grievanceService.create(formData);
          break;
      }
      setIsModalOpen(false);
      loadData();
    } catch (error) {
      alert('Error creating request');
    }
  };

  const renderTable = () => {
    switch (activeTab) {
      case 'attendance':
        return (
          <DataTable
            data={attendances}
            columns={[
              { key: 'date', label: 'Date', render: (val) => new Date(val).toLocaleDateString() },
              { key: 'status', label: 'Status' },
            ]}
            showActions={false}
          />
        );
      case 'gatepass':
        return (
          <DataTable
            data={gatePasses}
            columns={[
              { key: 'purpose', label: 'Purpose' },
              { key: 'outTime', label: 'Out Time', render: (val) => new Date(val).toLocaleString() },
              { key: 'inTime', label: 'In Time', render: (val) => val ? new Date(val).toLocaleString() : 'N/A' },
              { key: 'status', label: 'Status' },
            ]}
            showActions={false}
          />
        );
      case 'roomchange':
        return (
          <DataTable
            data={roomChanges}
            columns={[
              { key: 'currentRoomNumber', label: 'Current Room' },
              { key: 'requestedRoomNumber', label: 'Requested Room' },
              { key: 'reason', label: 'Reason' },
              { key: 'status', label: 'Status' },
            ]}
            showActions={false}
          />
        );
      case 'grievances':
        return (
          <DataTable
            data={grievances}
            columns={[
              { key: 'title', label: 'Title' },
              { key: 'description', label: 'Description', render: (val) => val.substring(0, 50) + '...' },
              { key: 'status', label: 'Status' },
              { key: 'createdDate', label: 'Created', render: (val) => new Date(val).toLocaleDateString() },
            ]}
            showActions={false}
          />
        );
      default:
        return <div>Select a tab</div>;
    }
  };

  return (
    <DashboardLayout menuItems={menuItems}>
      <div>
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-gray-900">Student Dashboard</h1>
          {student && (
            <p className="text-gray-600 mt-2">
              Welcome, {student.name} (Roll: {student.rollNumber})
            </p>
          )}
        </div>

        <div className="mb-4">
          <div className="flex space-x-2 border-b">
            {['attendance', 'gatepass', 'roomchange', 'grievances'].map((tab) => (
              <button
                key={tab}
                onClick={() => setActiveTab(tab)}
                className={`px-4 py-2 font-medium ${
                  activeTab === tab
                    ? 'border-b-2 border-indigo-600 text-indigo-600'
                    : 'text-gray-600 hover:text-gray-900'
                }`}
              >
                {tab.charAt(0).toUpperCase() + tab.slice(1)}
              </button>
            ))}
          </div>
        </div>

        {(activeTab === 'gatepass' || activeTab === 'roomchange' || activeTab === 'grievances') && (
          <div className="mb-4">
            <button
              onClick={handleCreate}
              className="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700"
            >
              {activeTab === 'gatepass' && 'Request Gate Pass'}
              {activeTab === 'roomchange' && 'Request Room Change'}
              {activeTab === 'grievances' && 'Submit Grievance'}
            </button>
          </div>
        )}

        {renderTable()}

        <Modal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          title={
            activeTab === 'gatepass' ? 'Request Gate Pass' :
            activeTab === 'roomchange' ? 'Request Room Change' :
            'Submit Grievance'
          }
        >
          <form onSubmit={handleSubmit} className="space-y-4">
            {activeTab === 'gatepass' && (
              <>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Purpose</label>
                  <input
                    type="text"
                    value={formData.purpose || ''}
                    onChange={(e) => setFormData({ ...formData, purpose: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Out Time</label>
                  <input
                    type="datetime-local"
                    value={formData.outTime || ''}
                    onChange={(e) => setFormData({ ...formData, outTime: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Expected In Time</label>
                  <input
                    type="datetime-local"
                    value={formData.inTime || ''}
                    onChange={(e) => setFormData({ ...formData, inTime: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  />
                </div>
              </>
            )}
            {activeTab === 'roomchange' && (
              <>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Requested Room</label>
                  <select
                    value={formData.requestedRoomId || ''}
                    onChange={(e) => setFormData({ ...formData, requestedRoomId: parseInt(e.target.value) })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  >
                    <option value="">Select Room</option>
                    {rooms.map((room) => (
                      <option key={room.roomId} value={room.roomId}>
                        {room.roomNumber} (Floor {room.floor}, Capacity: {room.capacity})
                      </option>
                    ))}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Reason</label>
                  <textarea
                    value={formData.reason || ''}
                    onChange={(e) => setFormData({ ...formData, reason: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    rows="4"
                    required
                  />
                </div>
              </>
            )}
            {activeTab === 'grievances' && (
              <>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Title</label>
                  <input
                    type="text"
                    value={formData.title || ''}
                    onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Description</label>
                  <textarea
                    value={formData.description || ''}
                    onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    rows="5"
                    required
                  />
                </div>
              </>
            )}
            <div className="flex justify-end space-x-2">
              <button
                type="button"
                onClick={() => setIsModalOpen(false)}
                className="px-4 py-2 border border-gray-300 rounded-md"
              >
                Cancel
              </button>
              <button
                type="submit"
                className="px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700"
              >
                Submit
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </DashboardLayout>
  );
};

export default StudentDashboard;



