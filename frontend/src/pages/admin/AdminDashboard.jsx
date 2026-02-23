import { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import DashboardLayout from '../../components/DashboardLayout';
import DataTable from '../../components/DataTable';
import Modal from '../../components/Modal';
import { studentService } from '../../services/studentService';
import { wardenService } from '../../services/wardenService';
import { attendanceService } from '../../services/attendanceService';
import { gatePassService } from '../../services/gatePassService';
import { roomChangeService } from '../../services/roomChangeService';
import { grievanceService } from '../../services/grievanceService';
import { roomService } from '../../services/roomService';

const AdminDashboard = () => {
  const location = useLocation();
  
  // Map URL paths to tab names
  const getTabFromPath = (path) => {
    if (path.includes('/students')) return 'students';
    if (path.includes('/wardens')) return 'wardens';
    if (path.includes('/attendance')) return 'attendance';
    if (path.includes('/gatepass')) return 'gatepass';
    if (path.includes('/roomchange')) return 'roomchange';
    if (path.includes('/grievances')) return 'grievances';
    if (path.includes('/dashboard')) return 'students'; // Default to students on dashboard
    return 'students'; // Default
  };
  
  const [activeTab, setActiveTab] = useState(getTabFromPath(location.pathname));
  const [students, setStudents] = useState([]);
  const [wardens, setWardens] = useState([]);
  const [attendances, setAttendances] = useState([]);
  const [gatePasses, setGatePasses] = useState([]);
  const [roomChanges, setRoomChanges] = useState([]);
  const [grievances, setGrievances] = useState([]);
  const [rooms, setRooms] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modalType, setModalType] = useState('');
  const [editingItem, setEditingItem] = useState(null);
  const [formData, setFormData] = useState({});

  const menuItems = [
    { path: '/admin/dashboard', label: 'Dashboard', icon: '🏠' },
    { path: '/admin/students', label: 'Manage Students', icon: '👥' },
    { path: '/admin/wardens', label: 'Manage Wardens', icon: '👨‍💼' },
    { path: '/admin/attendance', label: 'Attendance', icon: '📊' },
    { path: '/admin/gatepass', label: 'Gate Pass', icon: '🚪' },
    { path: '/admin/roomchange', label: 'Room Change', icon: '🏠' },
    { path: '/admin/grievances', label: 'Grievances', icon: '💬' },
  ];

  // Update activeTab when route changes
  useEffect(() => {
    const tab = getTabFromPath(location.pathname);
    setActiveTab(tab);
  }, [location.pathname]);

  useEffect(() => {
    loadData();
  }, [activeTab]);

  const loadData = async () => {
    try {
      switch (activeTab) {
        case 'students':
          setStudents(await studentService.getAll());
          break;
        case 'wardens':
          setWardens(await wardenService.getAll());
          break;
        case 'attendance':
          setAttendances(await attendanceService.getAll());
          break;
        case 'gatepass':
          setGatePasses(await gatePassService.getAll());
          break;
        case 'roomchange':
          setRoomChanges(await roomChangeService.getAll());
          break;
        case 'grievances':
          setGrievances(await grievanceService.getAll());
          break;
        case 'rooms':
          setRooms(await roomService.getAll());
          break;
      }
    } catch (error) {
      console.error('Error loading data:', error);
      const errorMessage = error.response?.data?.message || error.message || 'Error loading data';
      console.error('Full error:', error.response);
      alert(`Error loading data: ${errorMessage}\n\nCheck browser console (F12) for more details.`);
    }
  };

  const handleCreate = () => {
    setEditingItem(null);
    setFormData({});
    setModalType(activeTab);
    setIsModalOpen(true);
  };

  const handleEdit = (item) => {
    setEditingItem(item);
    setFormData(item);
    setModalType(activeTab);
    setIsModalOpen(true);
  };

  const handleDelete = async (item) => {
    if (!window.confirm('Are you sure you want to delete this item?')) return;

    try {
      switch (activeTab) {
        case 'students':
          await studentService.delete(item.studentId);
          break;
        case 'wardens':
          await wardenService.delete(item.wardenId);
          break;
        case 'attendance':
          await attendanceService.delete(item.attendanceId);
          break;
        case 'gatepass':
          await gatePassService.delete(item.requestId);
          break;
        case 'roomchange':
          await roomChangeService.delete(item.requestId);
          break;
        case 'grievances':
          await grievanceService.delete(item.grievanceId);
          break;
      }
      loadData();
    } catch (error) {
      alert('Error deleting item');
    }
  };

  const handleApprove = async (item) => {
    try {
      if (activeTab === 'gatepass') {
        await gatePassService.approve(item.requestId, 'Approved');
      } else if (activeTab === 'roomchange') {
        await roomChangeService.approve(item.requestId, 'Approved');
      } else if (activeTab === 'grievances') {
        await grievanceService.resolve(item.grievanceId, 'Resolved');
      }
      loadData();
    } catch (error) {
      alert('Error approving item');
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingItem) {
        // Update
        switch (activeTab) {
          case 'students':
            await studentService.update(editingItem.studentId, formData);
            break;
          case 'wardens':
            await wardenService.update(editingItem.wardenId, formData);
            break;
          case 'attendance':
            await attendanceService.update(editingItem.attendanceId, formData);
            break;
        }
      } else {
        // Create
        switch (activeTab) {
          case 'students':
            await studentService.create(formData);
            break;
          case 'wardens':
            // Map form data to match backend DTO (PascalCase)
            const wardenData = {
              Username: formData.username,
              Email: formData.email,
              Password: formData.password,
              Name: formData.name,
              Phone: formData.phone || null,
              Department: formData.department || null
            };
            await wardenService.create(wardenData);
            break;
          case 'attendance':
            await attendanceService.create(formData);
            break;
        }
      }
      setIsModalOpen(false);
      loadData();
    } catch (error) {
      console.error('Error saving item:', error);
      console.error('Full error response:', error.response?.data);
      
      let errorMessage = 'Error saving item';
      
      if (error.response?.data) {
        // Check for array of errors
        if (Array.isArray(error.response.data.errors)) {
          errorMessage = error.response.data.errors.join('; ');
        } 
        // Check for error text (string)
        else if (typeof error.response.data.error === 'string') {
          errorMessage = error.response.data.error;
        }
        // Check for message
        else if (error.response.data.message) {
          errorMessage = error.response.data.message;
        }
        // Check for errors object (from Identity)
        else if (error.response.data.errors && typeof error.response.data.errors === 'object') {
          const errorDescriptions = Object.values(error.response.data.errors)
            .flat()
            .map(err => err.description || err)
            .join('; ');
          errorMessage = errorDescriptions || errorMessage;
        }
      } else if (error.message) {
        errorMessage = error.message;
      }
      
      // Show user-friendly error message
      alert(`❌ ${errorMessage}\n\nPlease check:\n- Username must be unique\n- Email must be unique\n- Password must meet requirements (6+ chars, uppercase, lowercase, digit)`);
    }
  };

  const renderTable = () => {
    switch (activeTab) {
      case 'students':
        return (
          <DataTable
            data={students}
            columns={[
              { key: 'name', label: 'Name' },
              { key: 'rollNumber', label: 'Roll Number' },
              { key: 'roomNumber', label: 'Room' },
              { key: 'phone', label: 'Phone' },
            ]}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        );
      case 'wardens':
        return (
          <DataTable
            data={wardens}
            columns={[
              { key: 'name', label: 'Name' },
              { key: 'email', label: 'Email' },
              { key: 'phone', label: 'Phone' },
              { key: 'department', label: 'Department' },
            ]}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        );
      case 'attendance':
        return (
          <DataTable
            data={attendances}
            columns={[
              { key: 'studentName', label: 'Student' },
              { key: 'rollNumber', label: 'Roll Number' },
              { key: 'date', label: 'Date', render: (val) => new Date(val).toLocaleDateString() },
              { key: 'status', label: 'Status' },
            ]}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        );
      case 'gatepass':
        return (
          <DataTable
            data={gatePasses}
            columns={[
              { key: 'studentName', label: 'Student' },
              { key: 'purpose', label: 'Purpose' },
              { key: 'outTime', label: 'Out Time', render: (val) => new Date(val).toLocaleString() },
              { key: 'status', label: 'Status' },
            ]}
            onApprove={handleApprove}
            onDelete={handleDelete}
          />
        );
      case 'roomchange':
        return (
          <DataTable
            data={roomChanges}
            columns={[
              { key: 'studentName', label: 'Student' },
              { key: 'currentRoomNumber', label: 'Current Room' },
              { key: 'requestedRoomNumber', label: 'Requested Room' },
              { key: 'status', label: 'Status' },
            ]}
            onApprove={handleApprove}
            onDelete={handleDelete}
          />
        );
      case 'grievances':
        return (
          <DataTable
            data={grievances}
            columns={[
              { key: 'studentName', label: 'Student' },
              { key: 'title', label: 'Title' },
              { key: 'status', label: 'Status' },
              { key: 'createdDate', label: 'Created', render: (val) => new Date(val).toLocaleDateString() },
            ]}
            onApprove={handleApprove}
            onDelete={handleDelete}
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
          <h1 className="text-3xl font-bold text-gray-900">Admin Dashboard</h1>
        </div>

        <div className="mb-4">
          <div className="flex space-x-2 border-b">
            {['students', 'wardens', 'attendance', 'gatepass', 'roomchange', 'grievances'].map((tab) => (
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

        <div className="mb-4">
          <button
            onClick={handleCreate}
            className="bg-indigo-600 text-white px-4 py-2 rounded-md hover:bg-indigo-700"
          >
            Add New
          </button>
        </div>

        {renderTable()}

        <Modal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          title={editingItem ? 'Edit' : 'Create'}
        >
          <form onSubmit={handleSubmit} className="space-y-4">
            {activeTab === 'students' && (
              <>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Name</label>
                  <input
                    type="text"
                    value={formData.name || ''}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Roll Number</label>
                  <input
                    type="text"
                    value={formData.rollNumber || ''}
                    onChange={(e) => setFormData({ ...formData, rollNumber: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Room Number</label>
                  <input
                    type="text"
                    value={formData.roomNumber || ''}
                    onChange={(e) => setFormData({ ...formData, roomNumber: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Phone</label>
                  <input
                    type="text"
                    value={formData.phone || ''}
                    onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  />
                </div>
                {!editingItem && (
                  <>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Username</label>
                      <input
                        type="text"
                        value={formData.username || ''}
                        onChange={(e) => setFormData({ ...formData, username: e.target.value })}
                        className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                        required
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Email</label>
                      <input
                        type="email"
                        value={formData.email || ''}
                        onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                        className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                        required
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Password</label>
                      <input
                        type="password"
                        value={formData.password || ''}
                        onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                        className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                        required
                      />
                    </div>
                  </>
                )}
              </>
            )}
            {activeTab === 'wardens' && (
              <>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Name</label>
                  <input
                    type="text"
                    value={formData.name || ''}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Email</label>
                  <input
                    type="email"
                    value={formData.email || ''}
                    onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Phone</label>
                  <input
                    type="text"
                    value={formData.phone || ''}
                    onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Department</label>
                  <input
                    type="text"
                    value={formData.department || ''}
                    onChange={(e) => setFormData({ ...formData, department: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                  />
                </div>
                {!editingItem && (
                  <>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Username</label>
                      <input
                        type="text"
                        value={formData.username || ''}
                        onChange={(e) => setFormData({ ...formData, username: e.target.value })}
                        className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                        required
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Password</label>
                      <input
                        type="password"
                        value={formData.password || ''}
                        onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                        className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                        required
                      />
                    </div>
                  </>
                )}
              </>
            )}
            {activeTab === 'attendance' && (
              <>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Student ID</label>
                  <input
                    type="number"
                    value={formData.studentId || ''}
                    onChange={(e) => setFormData({ ...formData, studentId: parseInt(e.target.value) })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Date</label>
                  <input
                    type="date"
                    value={formData.date ? new Date(formData.date).toISOString().split('T')[0] : ''}
                    onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Status</label>
                  <select
                    value={formData.status || 'Present'}
                    onChange={(e) => setFormData({ ...formData, status: e.target.value })}
                    className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2"
                    required
                  >
                    <option value="Present">Present</option>
                    <option value="Absent">Absent</option>
                  </select>
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
                {editingItem ? 'Update' : 'Create'}
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </DashboardLayout>
  );
};

export default AdminDashboard;



