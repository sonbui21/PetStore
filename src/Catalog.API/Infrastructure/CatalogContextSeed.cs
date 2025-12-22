namespace Catalog.API.Infrastructure;

public partial class CatalogContextSeed(ILogger<CatalogContextSeed> logger) : IDbSeeder<CatalogContext>
{
    public async Task SeedAsync(CatalogContext context)
    {
        var baseS3Url = "https://pet-shop-21.s3.ap-southeast-1.amazonaws.com/products";

        await context.CatalogItems.ExecuteDeleteAsync();
        await context.CatalogCategories.ExecuteDeleteAsync();
        await context.CatalogItemOptions.ExecuteDeleteAsync();

        var categories = new List<CatalogCategory>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Index = 0,
                Name = "New Arrivals",
                Slug = "collections/new-at-petpal",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Index = 1,
                Name = "Beds & Blankets",
                Slug = "collections/beds",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Index = 2,
                Name = "Dog Food",
                Slug = "collections/dog-food",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Index = 3,
                Name = "Cat Food",
                Slug = "collections/cat-food",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Index = 4,
                Name = "Pet Toys",
                Slug = "collections/pet-toys",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Index = 5,
                Name = "Accessories",
                Slug = "collections/accessories",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Index = 6,
                Name = "Premium",
                Slug = "collections/premium",
            },
        };

        var items = new List<CatalogItem>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "large-washable-fluffy-orthopedic-soft-dog-sofa-bed-snnozy-dream",
                Title = "Large Washable Fluffy Orthopedic Soft Dog Pillow Dog Sofa Bed-Snoozy Dream",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item1-1.jpeg",
                    $"{baseS3Url}/item1-2.jpeg",
                    $"{baseS3Url}/item1-3.jpeg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Camel", "Dark Grey","Brown"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["M", "L","XL"]
                    },
                ],

            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "festive-classic-tartan-cozy-dog-anti-anxiety-calming-bed",
                Title = "Festive Classic Tartan Cozy Dog Anti-Anxiety Calming Bed",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item2-1.jpeg",
                    $"{baseS3Url}/item2-2.jpeg",
                    $"{baseS3Url}/item2-3.jpeg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Red", "Blue","Black"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["M", "L","XL"]
                    },
                ],
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "one-piece-cozy-flannel-sofa-protection-non-slip-couch-cover",
                Title = "One-Piece Cozy Flannel Sofa Protection Non-Slip Couch Cover",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item3-1.jpeg",
                    $"{baseS3Url}/item3-2.jpeg",
                    $"{baseS3Url}/item3-3.jpeg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Grey", "Khaki", "White"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["1-Seater", "2-Seater", "3-Seater"]
                    },
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "cream-coloured-large-plaid-square-pet-mat-bed",
                Title = "Cream-colored Large Plaid Square Fuzzy Pet Dog Mat Bed Couch Cover",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item4-1.jpg",
                    $"{baseS3Url}/item4-2.jpg",
                    $"{baseS3Url}/item4-3.jpg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Brown", "Black", "Grayish Blue"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["23.6x63 in", "23.6x70.9 in", "27.6x59.1 in"]
                    },
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "soft-and-waterproof-scratch-resistant-non-linting-throw-couch-cover",
                Title = "Soft and Waterproof Scratch-Resistant Non-Linting Throw Couch Cover",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item5-1.jpg",
                    $"{baseS3Url}/item5-2.jpg",
                    $"{baseS3Url}/item5-3.jpg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Grey", "Khaki", "White"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["1-Seater", "2-Seater", "3-Seater"]
                    },
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "honeycomb-pattern-water-resistant-stretch-full-cover-magic-couch-cover",
                Title = "Honeycomb Pattern Water-resistant Stretch Full-Cover Magic Couch Cover",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item6-1.jpg",
                    $"{baseS3Url}/item6-2.jpg",
                    $"{baseS3Url}/item6-3.jpg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Grey", "Khaki", "White"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["1-Seater", "2-Seater", "3-Seater"]
                    },
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "garden-chic-cotton-protective-couch-cover",
                Title = "Garden Chic Cotton Protective Couch Cover",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item7-1.jpeg",
                    $"{baseS3Url}/item7-2.jpeg",
                    $"{baseS3Url}/item7-3.jpeg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Grey", "Khaki", "White"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["1-Seater", "2-Seater", "3-Seater"]
                    },
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "portable-lesuire-outing-pet-bolster-dog-car-seat-bed",
                Title = "Portable Leisure Outing Pet Bolster Large Dog Car Seat Bed",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item8-1.jpeg",
                    $"{baseS3Url}/item8-2.jpeg",
                    $"{baseS3Url}/item8-3.jpeg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Grey", "Khaki", "White"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["1-Seater", "2-Seater", "3-Seater"]
                    },
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "large-reversible-all-season-indie-boho-pet-carrier-and-dog-snuggle-sleeping-bag-wondernap",
                Title = "Large Reversible All-Season Indie Boho Pet Carrier and Dog Snuggle Sleeping Bag - Wondernap",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item9-1.jpeg",
                    $"{baseS3Url}/item9-2.jpeg",
                    $"{baseS3Url}/item9-3.jpeg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Grey", "Khaki", "White"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["1-Seater", "2-Seater", "3-Seater"]
                    },
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Slug = "fluffy-cozy-calming-pet-blanket-car-seat-protector-cover-surestep",
                Title = "Fluffy Cozy Calming Pet Blanket Car Seat Protector Cover- Surestep",
                Price = 60M,
                CurrencyCode = "USD",
                ImagesUrl =
                [
                    $"{baseS3Url}/item10-1.jpeg",
                    $"{baseS3Url}/item10-2.jpeg",
                    $"{baseS3Url}/item10-3.jpeg"
                ],
                CatalogCategories =RandomCategoriesWithPremium(categories),
                CatalogItemOptions =
                [
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Color",
                        Values = ["Grey", "Khaki", "White"]
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Size",
                        Values = ["1-Seater", "2-Seater", "3-Seater"]
                    },
                ]
            }
        };


        foreach (var item in items)
        {
            item.CatalogItemVariants = BuildVariants(item);
        }
        await context.CatalogCategories.AddRangeAsync(categories);
        await context.CatalogItems.AddRangeAsync(items);
        logger.LogInformation("Seeded catalog with {NumItems} items", await context.CatalogItems.CountAsync());
        await context.SaveChangesAsync();
    }

    public static List<CatalogItemVariant> BuildVariants(CatalogItem item)
    {
        var options = item.CatalogItemOptions.ToList();
        var random = new Random();

        List<List<(string Name, string Value)>> combinations = [];

        foreach (var option in options)
        {
            if (combinations.Count == 0)
            {
                combinations = [.. option.Values.Select(v =>
                new List<(string, string)> { (option.Name, v) })];
                continue;
            }

            var next = new List<List<(string, string)>>();

            foreach (var existing in combinations)
            {
                foreach (var value in option.Values)
                {
                    next.Add(existing.Append((option.Name, value)).ToList());
                }
            }

            combinations = next;
        }

        // 👉 random 1 variant hết hàng
        var outOfStockIndex = random.Next(combinations.Count);

        return combinations.Select((selectedOptions, index) =>
        {
            var title = string.Join(" / ", selectedOptions.Select(o => o.Value));

            var priceOffsets = new[] { -10, -5, 0, 5, 10 };
            var offset = priceOffsets[random.Next(priceOffsets.Length)];
            var variantPrice = item.Price * (100 + offset) / 100;

            return new CatalogItemVariant
            {
                Id = Guid.NewGuid(),
                CatalogItemId = item.Id,
                Title = title,
                Price = Math.Round(variantPrice, 2),
                CurrencyCode = item.CurrencyCode,

                // ✅ đảm bảo mỗi item có đúng 1 variant hết hàng
                AvailableStock = index == outOfStockIndex
                    ? 0
                    : random.Next(1, 20), // tồn kho ngẫu nhiên > 0

                SelectedOptions = [.. selectedOptions.Select(o =>
                new CatalogItemVariantOption
                {
                    Id = Guid.NewGuid(),
                    Name = o.Name,
                    Value = o.Value
                })]
            };
        }).ToList();
    }

    public static List<CatalogCategory> RandomCategoriesWithPremium(
    List<CatalogCategory> allCategories,
    int randomCount = 2)
    {
        var premium = allCategories.Single(c => c.Slug == "collections/premium");

        var nonPremium = allCategories
            .Where(c => c.Id != premium.Id)
            .OrderBy(_ => Guid.NewGuid())
            .Take(randomCount)
            .ToList();

        var result = new List<CatalogCategory>
        {
            premium
        };
        result.AddRange(nonPremium);
        return result;
    }

}

