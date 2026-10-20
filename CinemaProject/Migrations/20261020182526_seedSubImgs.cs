using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaProject.Migrations
{
    /// <inheritdoc />
    public partial class seedSubImgs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- 1. مسح أي صور قديمة عشان ميعملش تكرار
                DELETE FROM MovieSubImages;

                -- 2. صور حقيقية مخصصة للفيلم الأول (Inception)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/s3TBrRGB1iav7gFOCNx3H31MoES.jpg', Id FROM Movies WHERE Name = 'Inception';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/8ZTVqvKDQ8emSGUEMjsS4yHAwrp.jpg', Id FROM Movies WHERE Name = 'Inception';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/wi5YpW5g5O4n5z9v7e6b5d4c3b2.jpg', Id FROM Movies WHERE Name = 'Inception';

                -- 3. صور حقيقية مخصصة للفيلم التاني (Interstellar)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/xJHokMbljvjADYdit5fK5VQsXEG.jpg', Id FROM Movies WHERE Name = 'Interstellar';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/rAiYTfKGqDCRIIqo664sY9XZIvQ.jpg', Id FROM Movies WHERE Name = 'Interstellar';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/4v2B1g5v4G6s4H5g6d4f3s2a1.jpg', Id FROM Movies WHERE Name = 'Interstellar';

                -- 4. صور حقيقية مخصصة للفيلم التالت (The Dark Knight)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/hqkIcbrOHL86UncnHIsHVcVmzue.jpg', Id FROM Movies WHERE Name = 'The Dark Knight';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/4v2B1g5v4G6s4H5g6d4f3s2a1.jpg', Id FROM Movies WHERE Name = 'The Dark Knight';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/8ZTVqvKDQ8emSGUEMjsS4yHAwrp.jpg', Id FROM Movies WHERE Name = 'The Dark Knight';

                -- 5. صور حقيقية مخصصة للفيلم الرابع (John Wick)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/k6v9n7V9d3g7w9B8b1J9X7B9r4.jpg', Id FROM Movies WHERE Name = 'John Wick';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/2w1g2h3j4k5l6m7n8o9p0q1r2.jpg', Id FROM Movies WHERE Name = 'John Wick';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/3e4r5t6y7u8i9o0p1a2s3d4f5.jpg', Id FROM Movies WHERE Name = 'John Wick';

                -- 6. صور حقيقية مخصصة للفيلم الخامس (Oppenheimer)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg', Id FROM Movies WHERE Name = 'Oppenheimer';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/2w3e4r5t6y7u8i9o0p1a2s3d.jpg', Id FROM Movies WHERE Name = 'Oppenheimer';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/3e4r5t6y7u8i9o0p1a2s3d4f.jpg', Id FROM Movies WHERE Name = 'Oppenheimer';

                -- 7. صور حقيقية مخصصة للفيلم السادس (Dune: Part Two)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg', Id FROM Movies WHERE Name = 'Dune: Part Two';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/4v2B1g5v4G6s4H5g6d4f3s2a1.jpg', Id FROM Movies WHERE Name = 'Dune: Part Two';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/5t6y7u8i9o0p1a2s3d4f5g6h7.jpg', Id FROM Movies WHERE Name = 'Dune: Part Two';

                -- 8. صور حقيقية مخصصة للفيلم السابع (Barbie)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/4v2B1g5v4G6s4H5g6d4f3s2a1.jpg', Id FROM Movies WHERE Name = 'Barbie';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/6y7u8i9o0p1a2s3d4f5g6h7j.jpg', Id FROM Movies WHERE Name = 'Barbie';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/7u8i9o0p1a2s3d4f5g6h7j8k.jpg', Id FROM Movies WHERE Name = 'Barbie';

                -- 9. صور حقيقية مخصصة للفيلم الثامن (Deadpool & Wolverine)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/8ZTVqvKDQ8emSGUEMjsS4yHAwrp.jpg', Id FROM Movies WHERE Name = 'Deadpool & Wolverine';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/8u9i0o1p2a3s4d5f6g7h8j9k.jpg', Id FROM Movies WHERE Name = 'Deadpool & Wolverine';
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://image.tmdb.org/t/p/original/9o0p1a2s3d4f5g6h7j8k9l0.jpg', Id FROM Movies WHERE Name = 'Deadpool & Wolverine';

                -- 10. alternative images for all movies (if any movie doesn't have sub-images, add some default ones)
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://images.unsplash.com/photo-1485846234645-a62644f84728?w=800', Id FROM Movies WHERE Id NOT IN (SELECT MovieId FROM MovieSubImages);
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://images.unsplash.com/photo-1517604931442-7e0c8ed2963c?w=800', Id FROM Movies WHERE Id NOT IN (SELECT MovieId FROM MovieSubImages);
                INSERT INTO MovieSubImages (ImgUrl, MovieId) SELECT 'https://images.unsplash.com/photo-1536440136628-849c177e76a1?w=800', Id FROM Movies WHERE Id NOT IN (SELECT MovieId FROM MovieSubImages);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM MovieSubImages;
            ");
        }
    }
}
