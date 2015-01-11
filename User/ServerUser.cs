using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace User
{
    public class ServerUser
    {
        public class UserState
        {
            public bool state;
            public EndPoint ipAddress;
        }
        public SortedDictionary<string, UserState> userList = new SortedDictionary<string, UserState>();

        public void LogOn(string fromName, EndPoint address)
        {
            UserState state = new UserState
            {
                state = true,
                ipAddress = address
            };

            if (userList.ContainsKey(fromName))
            {
                userList[fromName] = state;
            }
            else
            {
                userList.Add(fromName, state);
            }
        }

        public void LogOff(string fromName)
        {
            if (userList.ContainsKey(fromName))
            {
                userList[fromName].state = false;
            }
        }

        public EndPoint CheckLog(string fromName)
        {
            if (userList.ContainsKey(fromName))
            {
                return userList[fromName].ipAddress;
            }
            return null;
        }
    }
}
