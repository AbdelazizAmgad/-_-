using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaProject.Migrations
{
    /// <inheritdoc />
    public partial class seedhotmovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Movies (Name, Description, Price, Status, IsHot, [DateTime], MainImg, CategoryId, CinemaId) VALUES 
                ('Oppenheimer', 'The story of American scientist J. Robert Oppenheimer and his role in the development of the atomic bomb.', 18.00, 1, 1, '2025-02-01 19:00:00', 'https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg', 3, 1),
                ('Dune: Part Two', 'Paul Atreides unites with Chani and the Fremen while seeking revenge against the conspirators who destroyed his family.', 22.00, 1, 1, '2025-02-02 20:00:00', 'https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg', 2, 2),
                ('Barbie', 'Barbie suffers a crisis that leads her to question her world and her existence.', 12.00, 1, 0, '2025-02-03 18:30:00', 'https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg', 3, 3),
                ('Deadpool & Wolverine', 'Wolverine is recovering from his injuries when he crosses paths with the loudmouth, Deadpool. They team up to defeat a common enemy.', 16.50, 1, 1, '2025-02-04 21:30:00', 'https://image.tmdb.org/t/p/w500/8cdWjvZQUExUUTzyp4t6EDMubfO.jpg', 1, 1);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Movies WHERE Name IN ('Oppenheimer', 'Dune: Part Two', 'Barbie', 'Deadpool & Wolverine');
            ");
        }
    }
}
