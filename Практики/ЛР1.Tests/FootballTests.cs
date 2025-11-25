using Xunit;

namespace ЛР1.Tests
{
    public class FootballTests
    {
        [Fact]
        public void Football_Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            var name = "Football";
            var hardLvl = 7u;
            var teamName = "Team A";

            // Act
            var football = new Football(name, hardLvl, teamName);

            // Assert
            Assert.Equal(name, football.Name);
            Assert.Equal(hardLvl, football.HardLvl);
            Assert.Equal(11u, football.PlayersAmount); // Football всегда 11 игроков
            Assert.Equal(teamName, football.TeamName);
        }

        [Fact]
        public void Football_HardLvl_CanBeSet()
        {
            // Arrange
            var football = new Football("Football", 5, "Team");
            var newHardLvl = 8u;

            // Act
            football.HardLvl = newHardLvl;

            // Assert
            Assert.Equal(newHardLvl, football.HardLvl);
        }

        [Fact]
        public void Football_HardLvl_RejectsValuesGreaterThan10()
        {
            // Arrange
            var football = new Football("Football", 5, "Team");
            var initialHardLvl = football.HardLvl;

            // Act
            football.HardLvl = 15u; // Значение > 10

            // Assert
            Assert.Equal(initialHardLvl, football.HardLvl); // Должно остаться прежним
        }

        [Fact]
        public void Football_Name_CanBeChanged()
        {
            // Arrange
            var football = new Football("Football", 5, "Team");
            var newName = "Updated Football";

            // Act
            football.Name = newName;

            // Assert
            Assert.Equal(newName, football.Name);
        }

        [Fact]
        public void Football_PlayersAmount_IsAlways11()
        {
            // Arrange & Act
            var football = new Football("Football", 5, "Team");

            // Assert
            Assert.Equal(11u, football.PlayersAmount);
        }

        [Fact]
        public void Football_TeamName_CanBeChanged()
        {
            // Arrange
            var football = new Football("Football", 5, "Team A");
            var newTeamName = "Team B";

            // Act
            football.TeamName = newTeamName;

            // Assert
            Assert.Equal(newTeamName, football.TeamName);
        }
    }
}
