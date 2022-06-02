using NUnit.Framework;
using TripServiceKata.Trip;
using TripServiceKata.User;
using TripServiceKata.Exception;
using System.Collections.Generic;

namespace TripServiceKata.Tests
{
    [TestFixture]
    public class TripServiceTest
    {
        private static readonly User.User LOGGED_USER = new User.User();
        private static readonly User.User ANOTHER_USER = new User.User();
        private static readonly Trip.Trip TO_QUEBEC = new Trip.Trip();
        private static readonly Trip.Trip TO_BOSTON = new Trip.Trip();

        [Test]
        public void ShouldThrowExceptionWhenUserNotLoggedIn()
        {
            var sut = new TestableTripService(null);

            Assert.Throws<UserNotLoggedInException>(() => sut.GetTripsByUser(null));
        }

        [Test]
        public void ShouldReturnEmptyListIfNotFriend()
        {            
            var tripDAOSub = NSubstitute.Substitute.For<TripDAO>();
            var sut = new TripService(LOGGED_USER, tripDAOSub);

            var result = sut.GetTripsByUser(ANOTHER_USER);

            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnEmptyListIfFriendWithNoTrips()
        {
            User.User friend = new User.User();
            friend.AddFriend(LOGGED_USER);

            var tripDAOSub = NSubstitute.Substitute.For<TripDAO>();
            tripDAOSub.tripsByUser(friend).
            var sut = new TripService(LOGGED_USER, tripDAOSub);

            var result = sut.GetTripsByUser(friend);

            Assert.IsEmpty(result);
        }

        [Test]
        public void ShouldReturnTripsIfFriend()
        {
            User.User friend = new User.User();
            friend.AddFriend(LOGGED_USER);
            friend.AddTrip(TO_BOSTON);
            friend.AddTrip(TO_QUEBEC);
            var sut = new TestableTripService(LOGGED_USER);

            var result = sut.GetTripsByUser(friend);

            Assert.AreEqual(2, result.Count);
        }

        private class TestableTripService : TripService
        {
            private readonly User.User loggedUser;

            public TestableTripService(User.User loggedInUser)
                : base(loggedInUser, null)
            { }

            protected override List<Trip.Trip> FindTripsByUserInternal(User.User user)
            {
                return user.Trips();
            }
        }

    }
}
