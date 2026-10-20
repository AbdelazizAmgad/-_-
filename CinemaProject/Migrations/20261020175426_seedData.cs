using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaProject.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. مسح الداتا القديمة لو موجودة
                DELETE FROM ActorMovie;
                DELETE FROM MovieSubImages;
                DELETE FROM Movies;
                DELETE FROM Actors;
                DELETE FROM Cinemas;
                DELETE FROM Categories;

                -- 2. تصفير العدادت عشان تبدأ من 1
                DBCC CHECKIDENT ('Categories', RESEED, 0);
                DBCC CHECKIDENT ('Cinemas', RESEED, 0);
                DBCC CHECKIDENT ('Actors', RESEED, 0);
                DBCC CHECKIDENT ('Movies', RESEED, 0);
                DBCC CHECKIDENT ('MovieSubImages', RESEED, 0);

                -- 3. إضافة الأقسام
                INSERT INTO Categories (Name) VALUES 
                ('Action'), ('Sci-Fi'), ('Drama'), ('Thriller');

                -- 4. إضافة السينمات
                INSERT INTO Cinemas (Name, Img) VALUES 
                ('VOX Cinemas', 'https://images.unsplash.com/photo-1517604931442-7e0c8ed2963c?w=800'),
                ('IMAX Theater', 'https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=800'),
                ('Galaxy Cinema', 'https://images.unsplash.com/photo-1512149177596-f817c7ef5d4c?w=800');

                -- 5. إضافة الممثلين (لاحظ إن اسم العمود name بحرف صغير زي الداتابيز بتاعتك)
                INSERT INTO Actors (name, Img) VALUES 
                ('Leonardo DiCaprio', 'https://image.tmdb.org/t/p/w500/wo2hJpn04vbtmh0B9utCFdsQhxM.jpg'),
                ('Tom Hardy', 'https://image.tmdb.org/t/p/w500/d81K0RH8UX7tZj49tZaQhZ9ewH.jpg'),
                ('Matthew McConaughey', 'https://image.tmdb.org/t/p/w500/wJiGedOCZhwMx9DezY8uwbT1mMg.jpg'),
                ('Anne Hathaway', 'https://image.tmdb.org/t/p/w500/tLelKoPNiyJCSEtQTz1FGv4TLGc.jpg'),
                ('Keanu Reeves', 'https://image.tmdb.org/t/p/w500/4D0PpNI0kmP58hgrwGC3a37nzu2.jpg'),
                ('Christian Bale', 'https://image.tmdb.org/t/p/w500/b7fTC9ZFf3pw5A8hE1zP9p1g9h.jpg');

                -- 6. إضافة الأفلام
                INSERT INTO Movies (Name, Description, Price, Status, [DateTime], MainImg, CategoryId, CinemaId) VALUES 
                ('Inception', 'A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.', 15.99, 1, '2025-01-15 18:00:00', 'https://image.tmdb.org/t/p/w500/oYuLEt3zVCKq57qu2F8dT7NIa6f.jpg', 2, 1),
                ('Interstellar', 'A team of explorers travel through a wormhole in space in an attempt to ensure humanity''s survival.', 19.99, 1, '2025-01-16 20:30:00', 'https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg', 2, 2),
                ('The Dark Knight', 'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.', 14.50, 1, '2025-01-17 19:00:00', 'https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg', 1, 1),
                ('John Wick', 'An ex-hit-man comes out of retirement to track down the gangsters that killed his dog and took everything from him.', 12.00, 1, '2025-01-18 21:00:00', 'https://image.tmdb.org/t/p/w500/fZPSd91yGE9fCcCe6OoQr6E3Bev.jpg', 1, 3);

                -- 7. إضافة الصور الفرعية
                INSERT INTO MovieSubImages (ImgUrl, MovieId) VALUES 
                ('https://image.tmdb.org/t/p/original/s3TBrRGB1iav7gFOCNx3H31MoES.jpg', 1),
                ('https://image.tmdb.org/t/p/original/xJHokMbljvjADYdit5fK5VQsXEG.jpg', 2),
                ('https://image.tmdb.org/t/p/original/hqkIcbrOHL86UncnHIsHVcVmzue.jpg', 3),
                ('https://image.tmdb.org/t/p/original/k6v9n7V9d3g7w9B8b1J9X7B9r4.jpg', 4);

                -- 8. ربط الممثلين بالأفلام (ActorMovie)
                INSERT INTO ActorMovie (ActorsId, MoviesId) VALUES 
                (1, 1), (2, 1), -- Inception
                (3, 2), (4, 2), -- Interstellar
                (6, 3),         -- Dark Knight
                (5, 4);         -- John Wick
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ActorMovie;
                DELETE FROM MovieSubImages;
                DELETE FROM Movies;
                DELETE FROM Actors;
                DELETE FROM Cinemas;
                DELETE FROM Categories;
            ");
        }
    }
}
