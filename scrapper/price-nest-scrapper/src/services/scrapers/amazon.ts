import { chromium } from "playwright";

export default async function scrapeAmazon(productName: string) {
    const browser = await chromium.launch({ headless: true });
    const context = await browser.newContext({
        userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36",
        extraHTTPHeaders: { 'Accept-Language': 'pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7' } // Wymusza polską wersję językową
    });
    const page = await context.newPage();

    try {
        productName = productName.toUpperCase();
        console.log(`Scraping Amazon.pl for product: ${productName}`);
        await page.goto(`https://www.amazon.pl/s?k=${encodeURIComponent(productName)}`, { waitUntil: "domcontentloaded" });

        try {
            await page.waitForSelector('[data-component-type="s-search-result"]', { timeout: 5000 });
        } catch (e) {
            console.log("Amazon: Nie znaleziono kafelków wyszukiwania produktów.");
        }

        await page.mouse.wheel(0, 1200);
        await page.waitForTimeout(1000);

        const products = await page.$$eval('[data-component-type="s-search-result"]', (items) => {
            return items.map(item => {
                const titleEl = item.querySelector('h2 a span, h2 span, .a-size-base-plus');

                // Cena na Amazonie jest podzielona na część całkowitą i ułamkową, 
                // ale najprościej wyciągnąć ją z ukrytego tekstu dla czytników ekranu (.a-offscreen)
                const priceEl = item.querySelector('.a-price .a-offscreen, .a-price-whole');

                const imgEl = item.querySelector('img.s-image') as HTMLImageElement | null;
                const linkEl = item.querySelector('h2 a, a.a-link-normal') as HTMLAnchorElement | null;

                const rawTitle = titleEl?.textContent || "";
                let rawPrice = priceEl?.textContent || "";

                // Jeśli pobraliśmy tylko całkowitą część ceny, spróbuj dołączyć ułamkową
                if (rawPrice && !rawPrice.includes(",") && !rawPrice.includes("zł")) {
                    const fractionEl = item.querySelector('.a-price-fraction');
                    if (fractionEl) {
                        rawPrice = `${rawPrice},${fractionEl.textContent} zł`;
                    } else {
                        rawPrice = `${rawPrice} zł`;
                    }
                }

                let imageUrl = imgEl?.src || imgEl?.getAttribute('src') || "";

                let url = "";
                if (linkEl) {
                    url = linkEl.href || linkEl.getAttribute('href') || "";
                    if (url.startsWith('/')) url = `https://www.amazon.pl${url}`;
                }

                const cleanTitle = rawTitle.replace(/\s+/g, ' ').trim();
                const cleanPrice = rawPrice.replace(/\s+/g, ' ').trim();

                return {
                    title: cleanTitle,
                    price: cleanPrice,
                    imageUrl: imageUrl,
                    productUrl: url,
                    storeName: "Amazon"
                };
            }).filter(p => p.title && p.price);
        });

        return products;
    } catch (error) {
        console.error("Error on Amazon scraper:", error);
        return [];
    } finally {
        await browser.close();
    }
}