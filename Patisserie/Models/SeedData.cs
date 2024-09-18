using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Patisserie.Areas.Identity.Data;
using static System.Net.WebRequestMethods;
using Patisserie.Models;

namespace Patisserie.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<PatisserieUser>>();
            IdentityResult roleResult;
            IdentityResult roleResult1;
                var roleExist = await RoleManager.RoleExistsAsync("Admin");
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
                }
                var roleExist1 = await RoleManager.RoleExistsAsync("User");
                if (!roleExist1)
                {
                    roleResult1 = await RoleManager.CreateAsync(new IdentityRole("User"));
                }
            PatisserieUser user = await UserManager.FindByEmailAsync("admin@patisserie.com");
            if (user == null)
            {
                var User = new PatisserieUser();
                User.Email = "admin@patisserie.com";
                User.UserName = "admin@patisserie.com";
                string userPWD = "Admin1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Patisserie.Data.PatisserieContext(
            serviceProvider.GetRequiredService<DbContextOptions<Patisserie.Data.PatisserieContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Category.Any() || context.Flavour.Any() || context.Product.Any() || context.ProductFlavours.Any())
                {
                    return;
                }
                context.Category.AddRange(
                new Category { Name = "Cakes", CategoryImage= "https://img.freepik.com/premium-photo/front-view-chocolate-cake-with-chocolate_1013720-1301.jpg", Description = "Indulge in our exquisite range of cakes, crafted with the finest ingredients and a touch of artistry. Perfect for any occasion, our cakes promise to deliver both flavor and elegance. From rich, creamy cheesecakes to beautifully decorated celebration cakes, each creation is designed to bring joy and sweetness to your special moments." },
                new Category { Name = "Cookies", CategoryImage = "https://images.aws.nestle.recipes/resized/5b069c3ed2feea79377014f6766fcd49_Original_NTH_Chocolate_Chip_Cookie_1080_850.jpg", Description = "Treat yourself to our irresistible selection of cookies, freshly baked to perfection. From classic chocolate chip to delicate macarons, each cookie is a delightful bite of sweetness. Perfect for enjoying with a cup of coffee or as a special gift, our cookies are crafted with care and passion to satisfy every craving." },
                new Category { Name = "Cupcakes", CategoryImage = "https://www.iheartnaptime.net/wp-content/uploads/2022/10/chocolate-cupcakes-from-scratch.jpg", Description = "Indulge in our delightful assortment of cupcakes, each a miniature masterpiece of flavor and creativity. From classic favorites like vanilla and chocolate to gourmet varieties with decadent toppings, our cupcakes are perfect for any occasion. Whether you're celebrating a birthday, shower, or simply treating yourself, our cupcakes promise a sweet and satisfying experience that's as beautiful as it is delicious." },
                new Category { Name = "Desserts and Sweets", CategoryImage = "https://insanelygoodrecipes.com/wp-content/uploads/2021/02/Sweet-Homemade-Chocolate-Layered-Dessert.jpg", Description = "Our decadent selection of desserts and sweets is crafted to satisfy every sweet tooth. From creamy puddings and fluffy mousses to elegant panna cotta and rich cheesecakes, each dessert is a luxurious treat. Perfect for ending a meal or enjoying as a special treat throughout the day, our desserts promise to delight with every bite." }
                );
                context.SaveChanges();
                context.Flavour.AddRange(
                new Flavour { Name= "Chocolate", FlavourImage= "https://food.fnr.sndimg.com/content/dam/images/food/fullset/2018/4/12/0/FNM_050118-Chocolate-Candy-Bar-Layer-Cake_s4x3.jpg.rend.hgtvcom.616.462.suffix/1523547441314.jpeg", Description= "Rich and indulgent, our chocolate creations offer a decadent experience with every bite, perfect for chocolate lovers." },
                new Flavour { Name = "Vanilla", FlavourImage = "https://publish.purewow.net/wp-content/uploads/sites/2/2021/08/vanilla-desserts-cat.jpg?fit=728%2C524", Description = "Classic and timeless, our vanilla treats are delightfully aromatic with a smooth and creamy texture that never fails to please." },
                new Flavour { Name = "Black Forest", FlavourImage = "https://www.sainsburysmagazine.co.uk/media/4577/download/black-forest-gateau-560.jpg?v=1", Description = "Layers of chocolate sponge, whipped cream, and cherries create a harmonious blend of flavors reminiscent of the famous Black Forest cake." },
                new Flavour { Name = "Red Velvet", FlavourImage = "https://funcakes.com/content/uploads/2021/02/Red-Velvet-Cake-with-Fruit-960x960-c-default.jpg", Description = "A velvety-smooth texture with a hint of cocoa, our red velvet treats are topped with creamy frosting for a luxurious taste experience." },
                new Flavour { Name = "Coconut", FlavourImage = "https://www.homecookingadventure.com/wp-content/uploads/2017/02/Almond-Coconut-Cake-Raffaello-cake-main1.webp", Description = "Tropical and refreshing, our coconut treats bring a taste of paradise with their sweet, nutty flavor and delicate texture." },
                new Flavour { Name = "Caramel", FlavourImage = "https://stordfkenticomedia.blob.core.windows.net/df-us/rms/media/recipesmedia/recipes/retail/x17/2003/feb/17244-caramel-topped-ice-cream-dessert-600x600.jpg?ext=.jpg", Description = "Indulge in the velvety-smooth richness of our caramel creations, where buttery sweetness meets a hint of salt for a perfectly balanced flavor." },
                new Flavour { Name = "Blueberry", FlavourImage = "https://www.loveandoliveoil.com/wp-content/uploads/2015/08/blueberry-creme-friache-cheesecake2-1.jpg", Description = "Bursting with juicy sweetness, our blueberry treats showcase the vibrant flavors of fresh blueberries in every bite." },
                new Flavour { Name = "Lemon", FlavourImage = "https://www.southernliving.com/thmb/z706skTv8rLcnFwyTEnMzr_H5zQ=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/Southern-Living_27364_SR_Lemon-Lush_13190-fd96c709fc2946a7aeb5c869f9ad470e.jpg", Description = "Bright and zesty, our lemon treats offer a delightful burst of citrus flavor that's both refreshing and satisfying." }
               );
                context.SaveChanges();
                context.Product.AddRange(
                new Product
                {
                    Name="Chocolate and coconut cake",
                    Description= "Indulge in the decadent harmony of chocolate and coconut with our signature cake creation. Layers of moist chocolate sponge cake are generously filled and frosted with creamy coconut buttercream, creating a luscious blend of flavors and textures. Each bite offers a symphony of rich cocoa undertones balanced by the tropical sweetness of coconut, making it a perfect choice for any occasion, from birthdays to celebrations. Topped with toasted coconut flakes and drizzled with chocolate ganache, our Chocolate and Coconut Cake is a true delight for those who crave a luxurious yet comforting dessert experience.",
                    Price=30,
                    ImageUrl= "https://www.alsothecrumbsplease.com/wp-content/uploads/2019/03/Coconut-Chocolate-Cake_1.jpg",
                    CategoryId= context.Category.Single(d => d.Name == "Cakes").CategoryId
                },
                new Product
                {
                    Name = "Black forest cake",
                    Description = "Immerse yourself in the timeless elegance of our Black Forest Cake, a masterpiece that combines layers of moist chocolate sponge cake soaked in cherry liqueur with luscious whipped cream and sweet cherries. Each slice reveals a symphony of flavors, from the rich cocoa notes of the cake to the delicate sweetness of the cherries, harmoniously balanced by creamy layers of whipped cream. Adorned with dark chocolate shavings and garnished with fresh cherries, our Black Forest Cake is a celebration of indulgence and tradition, perfect for marking special occasions with its irresistible allure and unforgettable taste.",
                    Price = 28,
                    ImageUrl = "https://thescranline.com/wp-content/uploads/2023/06/BLACK-FOREST-CAKE-S-01.jpg",
                    CategoryId = context.Category.Single(d => d.Name == "Cakes").CategoryId
                },
                new Product
                {
                    Name = "Blueberry and lemon cookies",
                    Description = "Experience a burst of freshness in every bite with our blueberry and lemon cookies. These delicate treats combine zesty lemon zest and juicy blueberries, creating a perfect balance of tangy and sweet flavors. Each cookie is baked to golden perfection, offering a delightful combination of soft, chewy texture with bursts of fruitiness. Whether enjoyed as a refreshing snack or paired with your favorite tea, our blueberry and lemon cookies are sure to brighten your day with their vibrant taste.\r\n\r\n",
                    Price = 8,
                    ImageUrl = "https://myhappybakes.com/wp-content/uploads/2023/03/Lemon-Blueberry-Cookies-1.jpg",
                    CategoryId = context.Category.Single(d => d.Name == "Cookies").CategoryId
                },
                new Product
                {
                    Name = "Chocolate chip cookies",
                    Description = "Savor the nostalgic goodness of our chocolate chip cookies, baked to golden perfection with chunks of premium chocolate scattered throughout. Each bite offers a delightful contrast of crisp edges and soft, chewy centers, bursting with rich chocolate flavor. Whether enjoyed fresh from the oven or paired with a cold glass of milk, our chocolate chip cookies promise a comforting treat that brings warmth and joy to every moment.\r\n\r\n",
                    Price = 8,
                    ImageUrl = "https://www.onceuponachef.com/images/2021/11/Best-Chocolate-Chip-Cookies-760x950.jpg",
                    CategoryId= context.Category.Single(d => d.Name == "Cookies").CategoryId

                },
                new Product
                {
                    Name = "Vanilla caramel cake",
                    Description = "Delight in the exquisite blend of flavors in our Vanilla Caramel Cake with Coconut. Layers of tender vanilla sponge cake are lovingly filled and frosted with smooth caramel buttercream, creating a velvety sweetness that melts in your mouth. Between each layer lies a sprinkling of toasted coconut, adding a delightful crunch and tropical flair to every bite. Topped with a drizzle of rich caramel sauce and a sprinkling of coconut flakes, this cake is a symphony of vanilla's comforting notes, caramel's indulgent richness, and coconut's tropical essence. Perfect for any celebration or moment of indulgence, our Vanilla Caramel Cake with Coconut promises a sensory journey that satisfies the sweetest cravings.\r\n\r\n",
                    Price = 27,
                    ImageUrl = "https://cdn.pickuplimes.com/cache/eb/79/eb794b71ad1b1c00bb48d2ec01713e10.jpg",
                    CategoryId = context.Category.Single(d => d.Name == "Cakes").CategoryId
                },
                new Product
                {
                    Name = "Chocolate and blueberry cupcakes",
                    Description = "Indulge in the delightful harmony of rich chocolate and sweet blueberries with our decadent cupcakes. Moist chocolate cake, infused with hints of cocoa, forms the base, perfectly complemented by a luscious blueberry filling that bursts with fruity goodness in every bite. Topped with swirls of creamy chocolate frosting and garnished with fresh blueberries, each cupcake is a miniature masterpiece that promises a luxurious treat for chocolate and fruit enthusiasts alike. Whether enjoyed as a dessert or a special treat, our chocolate and blueberry cupcakes are sure to satisfy your craving for both indulgence and freshness.\r\n\r\n",
                    Price = 14,
                    ImageUrl = "https://i0.wp.com/www.themissinglokness.com/wp-content/uploads/2017/07/BJG_9297.jpg?resize=800%2C1200&ssl=1",
                    CategoryId = context.Category.Single(d => d.Name == "Cupcakes").CategoryId
                },
                new Product
                {
                    Name = "Chocolate caramel cupcakes",
                    Description= "Delight in the decadent blend of rich chocolate and velvety caramel with our indulgent cupcakes. Moist chocolate cake, infused with deep cocoa flavors, serves as the perfect base for a creamy caramel filling that oozes with every bite. Topped generously with smooth caramel buttercream frosting and drizzled with a caramel sauce, each cupcake is a luxurious treat that promises a symphony of sweet, buttery goodness. Whether enjoyed as a dessert or a special occasion delight, our chocolate caramel cupcakes offer a heavenly combination that satisfies even the most discerning sweet tooth.\r\n\r\n",
                    Price = 27,
                    ImageUrl = "https://chelsweets.com/wp-content/uploads/2022/12/chocolate-caramel-cupcake-finished-v5-735x980.jpg",
                    CategoryId = context.Category.Single(d => d.Name == "Cupcakes").CategoryId
                },
                new Product
                {
                    Name = "Vanilla and chocolate mousse",
                    Description= "Experience the blissful harmony of smooth vanilla and decadent chocolate in our delightful mousse. Layers of creamy vanilla mousse and rich chocolate mousse come together to create a luscious dessert that's both indulgent and satisfying. Each spoonful offers a perfect balance of sweet vanilla and deep chocolate flavors, complemented by a velvety texture that melts in your mouth. Whether served in elegant cups or as part of a layered cake, our vanilla and chocolate mousse promises a luxurious treat that captivates the senses and leaves you craving more.\r\n\r\n",
                    Price = 4,
                    ImageUrl = "https://www.archanaskitchen.com/images/archanaskitchen/1-Author/Priyanshu_Agarwal/Vanilla_Choco_Mousse.jpg",
                    CategoryId = context.Category.Single(d => d.Name == "Desserts and Sweets").CategoryId
                },
                new Product
                {
                    Name="Black Forest Mousse",
                    Description= "Immerse yourself in the luxurious flavors of our Black Forest mousse, inspired by the classic cake. Silky layers of dark chocolate mousse and fluffy whipped cream are delicately infused with cherry liqueur and adorned with tart cherry compote. Each spoonful offers a harmonious balance of rich chocolate, tangy cherries, and creamy texture, reminiscent of the beloved Black Forest cake. Topped with chocolate shavings and a cherry on top, our Black Forest mousse is a decadent dessert that captures the essence of indulgence and sophistication.\r\n\r\n",
                    Price=5,
                    ImageUrl= "https://img.taste.com.au/Rgs4ZsMJ/w720-h480-cfill-q80/taste/2016/11/black-forest-mousse-77851-1.jpeg",
                    CategoryId = context.Category.Single(d => d.Name == "Desserts and Sweets").CategoryId
                }
                );
                context.SaveChanges();
                context.ProductFlavours.AddRange(
                new ProductFlavour { ProductId = 1, FlavourId = 1 },
                new ProductFlavour { ProductId = 1, FlavourId = 5 },
                new ProductFlavour { ProductId = 2, FlavourId = 1 },
                new ProductFlavour { ProductId = 2, FlavourId = 3 },
                new ProductFlavour { ProductId = 3, FlavourId = 7 },
                new ProductFlavour { ProductId = 3, FlavourId = 8 },
                new ProductFlavour { ProductId = 4, FlavourId = 1 },
                new ProductFlavour { ProductId = 5, FlavourId = 2 },
                new ProductFlavour { ProductId = 5, FlavourId = 5 },
                new ProductFlavour { ProductId = 5, FlavourId = 6 },
                new ProductFlavour { ProductId = 6, FlavourId = 1 },
                new ProductFlavour { ProductId = 6, FlavourId = 7 },
                new ProductFlavour { ProductId = 7, FlavourId = 1 },
                new ProductFlavour { ProductId = 7, FlavourId = 6 },
                new ProductFlavour { ProductId = 8, FlavourId = 1 },
                new ProductFlavour { ProductId = 8, FlavourId = 2 },
                new ProductFlavour { ProductId = 9, FlavourId = 1 },
                new ProductFlavour { ProductId = 9, FlavourId = 3 }
                );
                context.SaveChanges();
            }
        }
    }
}
