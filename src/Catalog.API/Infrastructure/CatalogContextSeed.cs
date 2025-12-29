namespace Catalog.API.Infrastructure;

public partial class CatalogContextSeed(ILogger<CatalogContextSeed> logger) : IDbSeeder<CatalogContext>
{
    // ✅ chỉnh số lượng item muốn seed ở đây
    private const int RepeatPerTemplate = 12;

    // ✅ dùng Random.Shared để tránh bị trùng do new Random() liên tục
    private static readonly Random Random = Random.Shared;

    // ✅ mô tả 1 "template" để sinh nhiều item (tránh copy-paste dài)
    private sealed record CatalogItemTemplate(
        string Slug,
        string Title,
        string[] ImageFiles,
        (string Name, string[] Values)[] Options
    );

    public async Task SeedAsync(CatalogContext context)
    {
        var baseS3Url = "https://pet-shop-21.s3.ap-southeast-1.amazonaws.com/products";

        await context.CatalogItems.ExecuteDeleteAsync();
        await context.Categories.ExecuteDeleteAsync();
        await context.ItemOptions.ExecuteDeleteAsync();

        var categories = BuildCategories();
        var templates = BuildItemTemplates();

        // ✅ sinh nhiều items từ templates:
        // - Title luôn khác nhau (append số thứ tự)
        // - Images cố gắng KHÔNG trùng giữa các items (lấy từ pool ảnh toàn cục)
        var items = GenerateItems(
            templates: templates,
            categories: categories,
            baseS3Url: baseS3Url,
            repeatPerTemplate: RepeatPerTemplate
        );

        await context.Categories.AddRangeAsync(categories);
        await context.CatalogItems.AddRangeAsync(items);

        logger.LogInformation("Seeded catalog with {NumItems} items", await context.CatalogItems.CountAsync());
        await context.SaveChangesAsync();
    }

    private static List<Category> BuildCategories()
    {
        return new List<Category>
        {
            new() { Id = Guid.NewGuid(), Index = 0, Name = "New Arrivals",     Slug = "collections/new-at-petpal" },
            new() { Id = Guid.NewGuid(), Index = 1, Name = "Beds & Blankets",  Slug = "collections/beds" },
            new() { Id = Guid.NewGuid(), Index = 2, Name = "Dog Food",         Slug = "collections/dog-food" },
            new() { Id = Guid.NewGuid(), Index = 3, Name = "Cat Food",         Slug = "collections/cat-food" },
            new() { Id = Guid.NewGuid(), Index = 4, Name = "Pet Toys",         Slug = "collections/pet-toys" },
            new() { Id = Guid.NewGuid(), Index = 5, Name = "Accessories",      Slug = "collections/accessories" },
            new() { Id = Guid.NewGuid(), Index = 6, Name = "Premium",          Slug = "collections/premium" },
        };
    }

    private static List<CatalogItemTemplate> BuildItemTemplates()
    {
        return
        [
            new(
                Slug: "large-washable-fluffy-orthopedic-soft-dog-sofa-bed-snnozy-dream",
                Title: "Large Washable Fluffy Orthopedic Soft Dog Pillow Dog Sofa Bed-Snoozy Dream",
                ImageFiles: ["item1-1.jpeg", "item1-2.jpeg", "item1-3.jpeg"],
                Options:
                [
                    ("Color", new[] { "Camel", "Dark Grey", "Brown" }),
                    ("Size",  new[] { "M", "L", "XL" })
                ]
            ),
            new(
                Slug: "festive-classic-tartan-cozy-dog-anti-anxiety-calming-bed",
                Title: "Festive Classic Tartan Cozy Dog Anti-Anxiety Calming Bed",
                ImageFiles: new[] { "item2-1.jpeg", "item2-2.jpeg", "item2-3.jpeg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Red", "Blue", "Black" }),
                    ("Size",  new[] { "M", "L", "XL" })
                }
            ),
            new(
                Slug: "one-piece-cozy-flannel-sofa-protection-non-slip-couch-cover",
                Title: "One-Piece Cozy Flannel Sofa Protection Non-Slip Couch Cover",
                ImageFiles: new[] { "item3-1.jpeg", "item3-2.jpeg", "item3-3.jpeg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Grey", "Khaki", "White" }),
                    ("Size",  new[] { "1-Seater", "2-Seater", "3-Seater" })
                }
            ),
            new(
                Slug: "cream-coloured-large-plaid-square-pet-mat-bed",
                Title: "Cream-colored Large Plaid Square Fuzzy Pet Dog Mat Bed Couch Cover",
                ImageFiles: new[] { "item4-1.jpg", "item4-2.jpg", "item4-3.jpg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Brown", "Black", "Grayish Blue" }),
                    ("Size",  new[] { "23.6x63 in", "23.6x70.9 in", "27.6x59.1 in" })
                }
            ),
            new(
                Slug: "soft-and-waterproof-scratch-resistant-non-linting-throw-couch-cover",
                Title: "Soft and Waterproof Scratch-Resistant Non-Linting Throw Couch Cover",
                ImageFiles: new[] { "item5-1.jpg", "item5-2.jpg", "item5-3.jpg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Grey", "Khaki", "White" }),
                    ("Size",  new[] { "1-Seater", "2-Seater", "3-Seater" })
                }
            ),
            new(
                Slug: "honeycomb-pattern-water-resistant-stretch-full-cover-magic-couch-cover",
                Title: "Honeycomb Pattern Water-resistant Stretch Full-Cover Magic Couch Cover",
                ImageFiles: new[] { "item6-1.jpg", "item6-2.jpg", "item6-3.jpg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Grey", "Khaki", "White" }),
                    ("Size",  new[] { "1-Seater", "2-Seater", "3-Seater" })
                }
            ),
            new(
                Slug: "garden-chic-cotton-protective-couch-cover",
                Title: "Garden Chic Cotton Protective Couch Cover",
                ImageFiles: new[] { "item7-1.jpeg", "item7-2.jpeg", "item7-3.jpeg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Grey", "Khaki", "White" }),
                    ("Size",  new[] { "1-Seater", "2-Seater", "3-Seater" })
                }
            ),
            new(
                Slug: "portable-lesuire-outing-pet-bolster-dog-car-seat-bed",
                Title: "Portable Leisure Outing Pet Bolster Large Dog Car Seat Bed",
                ImageFiles: new[] { "item8-1.jpeg", "item8-2.jpeg", "item8-3.jpeg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Grey", "Khaki", "White" }),
                    ("Size",  new[] { "1-Seater", "2-Seater", "3-Seater" })
                }
            ),
            new(
                Slug: "large-reversible-all-season-indie-boho-pet-carrier-and-dog-snuggle-sleeping-bag-wondernap",
                Title: "Large Reversible All-Season Indie Boho Pet Carrier and Dog Snuggle Sleeping Bag - Wondernap",
                ImageFiles: new[] { "item9-1.jpeg", "item9-2.jpeg", "item9-3.jpeg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Grey", "Khaki", "White" }),
                    ("Size",  new[] { "1-Seater", "2-Seater", "3-Seater" })
                }
            ),
            new(
                Slug: "fluffy-cozy-calming-pet-blanket-car-seat-protector-cover-surestep",
                Title: "Fluffy Cozy Calming Pet Blanket Car Seat Protector Cover- Surestep",
                ImageFiles: new[] { "item10-1.jpeg", "item10-2.jpeg", "item10-3.jpeg" },
                Options: new (string, string[])[]
                {
                    ("Color", new[] { "Grey", "Khaki", "White" }),
                    ("Size",  new[] { "1-Seater", "2-Seater", "3-Seater" })
                }
            ),
        ];
    }

    private static List<CatalogItem> GenerateItems(
        List<CatalogItemTemplate> templates,
        List<Category> categories,
        string baseS3Url,
        int repeatPerTemplate
    )
    {
        var totalItems = templates.Count * repeatPerTemplate;

        // ✅ pool ảnh toàn cục để hạn chế trùng ảnh giữa các items
        // Lưu ý: nếu totalItems * 3 > số ảnh trong pool, việc trùng ảnh là KHÔNG TRÁNH KHỎI
        // => muốn "không trùng tuyệt đối" thì cần thêm ảnh vào S3 hoặc giảm RepeatPerTemplate.
        var imagePool = templates
            .SelectMany(t => t.ImageFiles.Select(f => $"{baseS3Url}/{f}"))
            .Distinct()
            .ToList();

        Shuffle(imagePool);

        var poolIndex = 0;

        var items = new List<CatalogItem>(totalItems);

        foreach (var template in templates)
        {
            for (var i = 0; i < repeatPerTemplate; i++)
            {
                // ✅ title khác nhau để hiển thị không bị trùng
                var suffix = $"{i + 1:00}";
                var uniqueTitle = $"{suffix} - {template.Title}";

                // ✅ slug đã unique (đảm bảo không trùng)
                var uniqueSlug = $"{template.Slug}-{i + 1}";

                // ✅ lấy 3 ảnh (cố gắng không trùng giữa các items)
                var images = NextImages(imagePool, ref poolIndex, count: 3);

                var item = new CatalogItem
                {
                    Id = Guid.NewGuid(),
                    Slug = uniqueSlug,
                    Title = uniqueTitle,
                    Price = 60M,
                    CurrencyCode = "USD",
                    Images = images,

                    Categories = RandomCategoriesWithPremium(categories),

                    ItemOptions = template.Options.Select(o => new ItemOption
                    {
                        Id = Guid.NewGuid(),
                        Name = o.Name,
                        Values = o.Values.ToList()
                    }).ToList()
                };

                item.ItemVariants = BuildVariants(item);
                items.Add(item);
            }
        }

        return items;
    }

    private static List<string> NextImages(List<string> pool, ref int index, int count)
    {
        var result = new List<string>(capacity: count);

        for (var i = 0; i < count; i++)
        {
            if (pool.Count == 0)
                break;

            // nếu pool không đủ ảnh cho tất cả items, ta vòng lại (sẽ bắt đầu trùng)
            if (index >= pool.Count)
                index = 0;

            result.Add(pool[index]);
            index++;
        }

        // ✅ đảm bảo trong 1 item không có ảnh trùng nhau
        // (trường hợp pool quá nhỏ)
        return result.Distinct().ToList();
    }

    private static void Shuffle<T>(IList<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public static List<ItemVariant> BuildVariants(CatalogItem item)
    {
        var options = item.ItemOptions.ToList();

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

        var outOfStockIndex = Random.Next(combinations.Count);

        return combinations.Select((selectedOptions, idx) =>
        {
            var title = string.Join(" / ", selectedOptions.Select(o => o.Value));

            var priceOffsets = new[] { -10, -5, 0, 5, 10 };
            var offset = priceOffsets[Random.Next(priceOffsets.Length)];
            var variantPrice = item.Price * (100 + offset) / 100;

            return new ItemVariant
            {
                Id = Guid.NewGuid(),
                CatalogItemId = item.Id,
                Title = title,
                Price = Math.Round(variantPrice, 2),

                AvailableStock = idx == outOfStockIndex ? 0 : Random.Next(1, 20),

                Options = [.. selectedOptions.Select(o =>
                    new ItemVariantOption
                    {
                        Id = Guid.NewGuid(),
                        Name = o.Name,
                        Value = o.Value
                    })]
            };
        }).ToList();
    }

    public static List<Category> RandomCategoriesWithPremium(
        List<Category> allCategories,
        int randomCount = 2
    )
    {
        var premium = allCategories.Single(c => c.Slug == "collections/premium");

        var nonPremium = allCategories
            .Where(c => c.Id != premium.Id)
            .OrderBy(_ => Guid.NewGuid())
            .Take(randomCount)
            .ToList();

        var result = new List<Category> { premium };
        result.AddRange(nonPremium);
        return result;
    }
}
