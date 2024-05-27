using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    public partial class FixingArchitecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identification of 3D Model")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of 3D Model"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "CreationDate of 3D Model"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Url of 3D Model"),
                    Category = table.Column<int>(type: "int", nullable: false, comment: "Category of 3D Model")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identification of User")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of User")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identification of Order")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CadId = table.Column<int>(type: "int", nullable: false, comment: "Identification of 3D model"),
                    BuyerId = table.Column<int>(type: "int", nullable: false, comment: "Identification of User"),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false, comment: "Description of Order"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of Order")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Cads_CadId",
                        column: x => x.CadId,
                        principalTable: "Cads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_User_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cads",
                columns: new[] { "Id", "Category", "CreationDate", "Name", "Url" },
                values: new object[,]
                {
                    { 1, 0, null, "Toothless", "46fe9e9caf84495baa4d528bb33b1b34" },
                    { 2, 0, null, "Baby Yoda", "0950c0ef766b4d93babd3a0f0cd4ed0e" },
                    { 3, 0, null, "Son Goku", "bb96bd63cc90439daa7fd37ee5daa881" },
                    { 4, 0, null, "MK Mobile Klassic Smoke", "d91a0437d3a74a7e8a3d9f8c1162bfc3" },
                    { 5, 0, null, "Daft Punk", "4e1dfa109e8b40a9abb7bbdb0b105e7a" },
                    { 6, 0, null, "Verne", "5178e98aaa6a4d9d9db7a0d86b52b40c" },
                    { 7, 0, null, "Bodybuilder Doge", "c9e0581f7d2d4882b7f47aec5199325c" },
                    { 8, 0, null, "Superman", "2c65245c84d04ff8835c165c728bbd53" },
                    { 9, 0, null, "Chica", "2f5d647986ed44058b0c16c061998739" },
                    { 10, 0, null, "Minecraft skin", "fa967cc9d829475b8c8f830b416842ef" },
                    { 11, 1, null, "Butterfly", "d45ea22770f44818949e8933b853c6c4" },
                    { 12, 1, null, "Jellyfish", "81be051e683646d3922b2f6e71eafa11" },
                    { 13, 1, null, "Horse", "15e0faf8e6c44dfea3f462d0ea2de2a2" },
                    { 14, 1, null, "Chameleon", "7b02c861d6384fb58ac14fcd54d98475" },
                    { 15, 1, null, "T-rex", "e18c433cdd1c49f8ac152348b7384037" },
                    { 16, 1, null, "Rhino", "8e97b62a90f44ce19ea9e3fd421f55b4" },
                    { 17, 1, null, "Shark", "9fcd3e15d0494428b4466cac33587009" },
                    { 18, 1, null, "Crab", "a222d47f0465494193f71943bb7048b6" },
                    { 19, 1, null, "Blobfish", "a0e3375984274cf8b7cdc5a933c0629a" },
                    { 20, 1, null, "Turtle", "98642490cc824df6b6101c85685cd0fe" },
                    { 21, 2, null, "Korean Dragon", "a2b3bc23d51f46ba9b6108348f0c1098" },
                    { 22, 2, null, "Meji man smoking", "c03779126db440a4a2067dd8b2f2fa1c" },
                    { 23, 2, null, "Cute Mushroom", "ffc370ddc6d542d590b9f503d0892ce0" },
                    { 24, 2, null, "Indian God", "5a696c0af274432cb91b6640f50f1e2c" },
                    { 25, 2, null, "One Piece Ship", "fbb3018c6ed64f36b482beaf12a9598f" },
                    { 26, 2, null, "Parking lot", "28dd1cc6c5a84140aa93fe315ca0e344" },
                    { 27, 2, null, "ARC-170 Starfighter", "06c7a3a6139249b4ae0a180ff9d82fe2" },
                    { 28, 2, null, "Aladin Forbidden Treasure", "b6b2b65fd7ae4424a73fde3eccb40558" },
                    { 29, 2, null, "Witcher Sword", "a14077a1620c43b7ae08ba1f7e5f9874" },
                    { 30, 2, null, "Robots", "1300eb8e684b42868bda89657cac1605" },
                    { 31, 3, null, "Chinese Old Wall Building", "70fda0cf110c4ccbb7de300560fc35a7" },
                    { 32, 3, null, "Old Chinese Roof House", "7d039b0a0db148cf919895668acea0f7" },
                    { 33, 3, null, "DAE Diroma Rustborn", "fb5b57f347064e73a494ecc52bc05498" },
                    { 34, 3, null, "Catholic Church", "3c17b4155d7f40448f528cbbb5f14910" },
                    { 35, 3, null, "Windmill", "e9667197a82d4ea49319e89bdea87005" },
                    { 36, 3, null, "Old Japanese Temple", "b58ace5581fd41c4adc0bf482b4eef35" },
                    { 37, 3, null, "Temple Environment", "b48e325111414dc087001aba3b84632a" },
                    { 38, 3, null, "Grandma's House", "edae469d3d674f7b885f8f8c523f0461" },
                    { 39, 3, null, "G-Toilet 4.0 V3", "c7342e31cb3f4347aa1e529e42016a02" },
                    { 40, 3, null, "Temple", "6cee019036f748b888d035e010a24bf2" },
                    { 41, 4, null, "Mermaid", "f29e201673ec46ebba5c91cabee8a9f8" },
                    { 42, 4, null, "Demon Mask", "8f25337b683d415e8f393eff60bc2aa1" }
                });

            migrationBuilder.InsertData(
                table: "Cads",
                columns: new[] { "Id", "Category", "CreationDate", "Name", "Url" },
                values: new object[,]
                {
                    { 43, 4, null, "Gun", "1fb17a68da1543899ef4ed042d334384" },
                    { 44, 4, null, "Biomechanical whale", "daaaf76c7d9e45618b5da7bf8f08e034" },
                    { 45, 4, null, "Wish-granting dragon", "35d9c60b0fdf440a9ee44f9481b3173e" },
                    { 46, 4, null, "Pokemon", "d1eac757cd234d85b8f010252f740417" },
                    { 47, 4, null, "Angel of Death", "9755efc92e2847e198d28852393bfe5e" },
                    { 48, 4, null, "Neytiri", "ef501601f20c4a759a804a86c70e2cbb" },
                    { 49, 4, null, "Witcher Sword", "a14077a1620c43b7ae08ba1f7e5f9874" },
                    { 50, 4, null, "Mystic Abyss Whale", "6c2067c38e324e03a5a4a5cbc8022c2c" },
                    { 51, 5, null, "Purple Down Coat", "482f2f6b0724424093a958cbcb65ca2c" },
                    { 52, 5, null, "Stripped Fedora Hat", "3d0638441bf9415ca9800ef498299895" },
                    { 53, 5, null, "Sunglasses", "3b31fcf2f9e54b9ca86e0e29ca86d3fb" },
                    { 54, 5, null, "Pikachu hoodie", "026fd91fc38d47c2b2f27ecdd1d8fb69" },
                    { 55, 5, null, "Dragon New Year Dress", "52a260fd27dd4d24beab51cffb6da0f9" },
                    { 56, 5, null, "Purple Senate Gown", "8aa82542e714477b85e46ae958ad8fa9" },
                    { 57, 5, null, "White shirt black leather skirt outfit", "9f9e3d05217a4f969cd08224ad0b0aee" },
                    { 58, 5, null, "Green and white hoodie", "af100701826e4bb9abc1ff1a1d24ab1e" },
                    { 59, 5, null, "School Uniform", "887e3a1705f040bc91597e1fad47a7c0" },
                    { 60, 5, null, "Roman style outfits", "629f5466d75f42a4abd377a951b5574e" },
                    { 61, 6, null, "Forest", "15e0faf8e6c44dfea3f462d0ea2de2a2" },
                    { 62, 6, null, "Ocean wave", "f86f444dc7cc4458a0140f05a03c5896" },
                    { 63, 6, null, "Mountain", "aa0ce50cda2847e3b417c2d48253a1e5" },
                    { 64, 6, null, "Hanging pots", "f7dc21a8f90f49069295f2d6aa1b1334" },
                    { 65, 6, null, "Plants", "e515bae7a87740f1b8cc6bdc5b9821fa" },
                    { 66, 6, null, "Trees", "e38c9ccc28994580b27bc50871bed80f" },
                    { 67, 6, null, "Water Lily", "70041f31b7224d73800ee0e485757a08" },
                    { 68, 6, null, "Earth's Core", "44c547c28fbf46f4ab430bf1398f5fdb" },
                    { 69, 6, null, "Fern Forest", "e3fb505d5f6d47118da83ebacf92929c" },
                    { 70, 6, null, "Mushrooms", "27dc43f90c5f463d91a762e4d1d099e5" },
                    { 71, 7, null, "Drone", "cfef32de2c704b758a0d80166f664302" },
                    { 72, 7, null, "iPhone 15 Pro", "18c0acd4c8a04a50ada94b99b1ed8a2c" },
                    { 73, 7, null, "USB", "3682b098a3df42c188ddde947d674740" },
                    { 74, 7, null, "iPad", "7af563e18e5145deb46d85c33afe6ce0" },
                    { 75, 7, null, "Batteries", "eefe894897f746f689b899f80b15d325" },
                    { 76, 7, null, "PS5", "54810f0b23a441ca93f44a8b06ec937b" },
                    { 77, 7, null, "Casio Watch", "e8e86c83768e40d2b2349355d6f2ed25" },
                    { 78, 7, null, "Camera", "d2e75e8571dc496f80db1965beb6467d" },
                    { 79, 7, null, "Tape recorder", "5ea4230620e44cad93d72df5a098425a" },
                    { 80, 7, null, "Bulb", "4c064f93de0c46b981a92c94d629c64d" },
                    { 81, 8, null, "Old Police Car", "011bdbb31ba641aa9c9912df4ce1ff99" },
                    { 82, 8, null, "Cybertruck", "5a86defda4b24836a504fe5e597fdb17" },
                    { 83, 8, null, "School Bus", "f18faa73da8c4ee3a19e8db1d4ae4cd9" },
                    { 84, 8, null, "Nissan 200SX S13", "151053ad7e0a4fcf944a7b523722dad6" }
                });

            migrationBuilder.InsertData(
                table: "Cads",
                columns: new[] { "Id", "Category", "CreationDate", "Name", "Url" },
                values: new object[,]
                {
                    { 85, 8, null, "Futuristic ZAZ Zaporozhec", "a0910debd9364cc1b8a698d2862e2f99" },
                    { 86, 8, null, "Mercedes", "18b16cf5c8574f8f93c5b04b3b945ef1" },
                    { 87, 8, null, "Russian motorcycle", "d5d366471b4c4d4c9365d52b5c963c13" },
                    { 88, 8, null, "F-35C Lightning II", "1b187b18ce1541e2ae3321065ac61cbe" },
                    { 89, 8, null, "Herbe", "beac1b607f2544708891b2491886e87f" },
                    { 90, 8, null, "Tesla Model X", "36e9d3598c554bb69f3d9cd00e161818" },
                    { 91, 9, null, "Chroma Squares", "31967e23bce548a19f3b6acf0be42747" },
                    { 92, 9, null, "Vector Prism", "913759bdac3b4084b46ddf3b40d12f82" },
                    { 93, 9, null, "Rigged Torus", "4e5d1d892c0e4d468f254ec3d0707bd4" },
                    { 94, 9, null, "Aurora Somnium", "dc45cd579572424d8793b3c7241618c3" },
                    { 95, 9, null, "Core Rotation", "42ed6690184249f3b8343204406729f4" },
                    { 96, 9, null, "Inner Reflection", "1dd1dac603634aa3b5f150ac91471866" },
                    { 97, 9, null, "The Call", "e92d198bc95c4fb7929b2605c919ad3d" },
                    { 98, 9, null, "Spheres and Torus", "8b94ad7d325f420fa5b71ae6b550f03c" },
                    { 99, 9, null, "Cradled by the void", "36a66e98adad4daf8d434d96bfbcb762" },
                    { 100, 9, null, "Tentakulum", "dd0dd9f4fb35422e9dd15c2a443dcd39" },
                    { 101, 10, null, "Muscular Male", "c1b72483e097443fbf0a94ddac01251f" },
                    { 102, 10, null, "Circular System", "e7abdabd3a6d422cb25e6e63b45b1ab0" },
                    { 103, 10, null, "Digestive System", "758773af9d6c4368a5549bef536484a9" },
                    { 104, 10, null, "Muscle system", "7ea21567ff9942bf9511e2d99efe85d9" },
                    { 105, 10, null, "Mouth", "d92cfd5873ac43299c7b64cdf9725526" },
                    { 106, 10, null, "Heart", "775d6629622740de8a5ed61a959c7506" },
                    { 107, 10, null, "Eye", "6adbd6538cd146d484c9ad950be69aa5" },
                    { 108, 10, null, "Skull", "2d6feb90e20d4389afb2b885eece0a48" },
                    { 109, 10, null, "Ear", "4f5438fc9337454587ec4a2c30c8c42f" },
                    { 110, 10, null, "Blood vessels", "c26e0f2e75fe4087aa8c4ecee2cad1aa" },
                    { 111, 11, null, "Hot Dog", "7d32ac170c274d08b29993bb61192fe2" },
                    { 112, 11, null, "Donut", "7f0b1caa6f7e448d9ce33cad8b255c2b" },
                    { 113, 11, null, "Milka Chocolate", "4455647163644960b4851ccd357089c1" },
                    { 114, 11, null, "Chicken Wing", "7cc9b52f39c44e2098fe7d4cf2d25725" },
                    { 115, 11, null, "Cake", "d8b2c2cc92914fefa0aac855fc434e02" },
                    { 116, 11, null, "Chips", "2342efc8c65a431db89747964e96f929" },
                    { 117, 11, null, "Waffles", "37e628439c5a47c4aed209c76b442960" },
                    { 118, 11, null, "Burger", "346ebe0033ba48f7bc2e10d44a5b482f" },
                    { 119, 11, null, "Muffins", "307dc81f3890494487ff2c7a12a2afb8" },
                    { 120, 11, null, "Lollipop", "3071e2c6382842509b17f2535224444d" },
                    { 121, 12, null, "Table with chairs", "8f2f097e2eda40c691956ad8de31ae8b" },
                    { 122, 12, null, "Knife", "2be56aac48d042449a89cf5c74aae4bf" },
                    { 123, 12, null, "Desk", "7d42419d4a034786b04374cd313ffb64" },
                    { 124, 12, null, "Couches", "b87c52f6f86642dc8c7bc8e321ac3a7b" },
                    { 125, 12, null, "Clock", "73ee073c9c0e4f82ac5e4357b18a220a" },
                    { 126, 12, null, "Carpentary tools", "b910747a856b40d1b85f1cfb1123ce4f" }
                });

            migrationBuilder.InsertData(
                table: "Cads",
                columns: new[] { "Id", "Category", "CreationDate", "Name", "Url" },
                values: new object[,]
                {
                    { 127, 12, null, "Candle holder", "2238c4e2fd2945a89b69d26b3a362b2f" },
                    { 128, 12, null, "Bookshelf", "dd9df1268f954a18b248a6d321aadab5" },
                    { 129, 12, null, "Household items", "31c376a493914f5d90f1e2894bd33399" },
                    { 130, 12, null, "Viking war horn", "82a543e0ddc74156b4ded2297cfa41d0" },
                    { 131, 13, null, "Gym Machines", "0582bd07f01b49b09b3552553c3467b8" },
                    { 132, 13, null, "Ping Pong", "8e47acdad1824af79e0ee17267cf219b" },
                    { 133, 13, null, "Dumbell", "e4fb3a53b8f64103b223a73877e2e76c" },
                    { 134, 13, null, "Billiard", "8942a97e5b034d6595ae8861a9b17090" },
                    { 135, 13, null, "Barbell", "45aa468ba3ff4e1cb6f3fa6022f62c2c" },
                    { 136, 13, null, "Balls", "a1ca652635c64fbeab407afaab798f50" },
                    { 137, 13, null, "Boxing gloves", "4c0fc8ab91c74b73a8ffc58b8dc0cacd" },
                    { 138, 13, null, "Azteca Mexico Football", "880bcabd62544bf3ab5246af0721ddc8" },
                    { 139, 13, null, "Boxing bag", "5e61a526b82149d4a536ccd0208677a0" },
                    { 140, 13, null, "Tennis Rackets", "1589dccdc3cf4ff5af17fe975b33650e" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CadId",
                table: "Orders",
                column: "CadId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Cads");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
