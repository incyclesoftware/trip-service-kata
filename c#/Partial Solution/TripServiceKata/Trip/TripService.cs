using System.Collections.Generic;
using TripServiceKata.Exception;
using TripServiceKata.User;

namespace TripServiceKata.Trip
{
    public class TripService
    {
        private readonly User.User loggedInUser;
        private readonly TripDAO tripDAO;

        public TripService(User.User loggedInUser, TripDAO tripDAO)
        {
            this.loggedInUser = loggedInUser;
            this.tripDAO = tripDAO;
        }

        public List<Trip> GetTripsByUser(User.User user)
        {
            List<Trip> tripList = new List<Trip>();
            
            bool isFriend = false;
            if (loggedInUser != null)
            {
                foreach (User.User friend in user.GetFriends())
                {
                    if (friend.Equals(loggedInUser))
                    {
                        isFriend = true;
                        break;
                    }
                }
                if (isFriend)
                {
                    tripList = FindTripsByUserInternal(user);
                }
                return tripList;
            }
            else
            {
                throw new UserNotLoggedInException();
            }
        }

        protected virtual List<Trip> FindTripsByUserInternal(User.User user)
        {
            return tripDAO.tripsByUser(user);
        }
    }
}
