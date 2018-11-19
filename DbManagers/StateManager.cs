using IPPhoneModel.EnumTypes;

namespace DbManagers
{
    /// <summary>
    /// 设备状态管理器
    /// </summary>
    public static class StateManager
    {
        public static State GetState(int stateid) 
        {
            switch (stateid) 
            {
                case 1:
                    return State.STAT_DEVICE_OFFLINE;
                case 2:
                    return State.STAT_DEVICE_ONLINE;
                case 3:
                    return State.STAT_DEVICE_TALKING;
                default:
                    return State.STAT_INVALID;
            }
        }
    }
}
