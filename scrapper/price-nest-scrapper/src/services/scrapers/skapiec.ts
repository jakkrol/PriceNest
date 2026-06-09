import { chromium } from "playwright";

export default async function scrapeSkapiec(productName: string) {
    // const browser = await chromium.launch({ headless: true });
    // const context = await browser.newContext({
    //     userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36"
    // });
    // const page = await context.newPage();

    // try {
    //     console.log(`Scraping Skapiec for product: ${productName}`);
    //     // Skąpiec używa prostego query stringa ?q=
    //     await page.goto(`https://www.skapiec.pl/szukaj?q=${encodeURIComponent(productName)}`, { waitUntil: "domcontentloaded" });

    //     try {
    //         // Czekamy na główny kontener ofert lub pojedyncze karty
    //         await page.waitForSelector('[data-component="product-card"], .component-product-card', { timeout: 5000 });
    //     } catch (e) {
    //         console.log("Skapiec: Nie znaleziono standardowej listy produktów lub układ strony się różni.");
    //     }

    //     // Przewijamy, by dociągnąć obrazki z lazy-loadingu
    //     await page.mouse.wheel(0, 1200);
    //     await page.waitForTimeout(1000);

    //     const products = await page.$$eval('[data-component="product-card"], .component-product-card, .box-product-card', (items) => {
    //         return items.map(item => {
    //             // Selektory tytułu i ceny (dopasowane pod aktualny widok Skąpca)
    //             const titleEl = item.querySelector('h2, .product-name, .title');
    //             const priceEl = item.querySelector('.price, [class*="price"]');
    //             const imgEl = item.querySelector('img') as HTMLImageElement | null;
    //             const linkEl = item.querySelector('a') as HTMLAnchorElement | null;

    //             const rawTitle = titleEl?.textContent || "";
    //             const rawPrice = priceEl?.textContent || "";

    //             // Wyciąganie obrazka z uwzględnieniem lazy load
    //             let imageUrl = "";
    //             if (imgEl) {
    //                 imageUrl = imgEl.getAttribute('data-src') || imgEl.getAttribute('lazy-src') || imgEl.src || "";
    //                 if (imageUrl.startsWith('//')) imageUrl = `https:${imageUrl}`;
    //             }

    //             // Wyciąganie bezpośredniego linku
    //             let url = "";
    //             if (linkEl) {
    //                 url = linkEl.href || linkEl.getAttribute('href') || "";
    //                 if (url.startsWith('/')) url = `https://www.skapiec.pl${url}`;
    //             }

    //             const cleanTitle = rawTitle.replace(/\s+/g, ' ').trim();
    //             const cleanPrice = rawPrice.replace(/\s+/g, ' ').trim();

    //             return {
    //                 title: cleanTitle,
    //                 price: cleanPrice,
    //                 imageUrl: imageUrl,
    //                 productUrl: url,
    //                 storeName: "Skąpiec"
    //             };
    //         }).filter(p => p.title && p.price);
    //     });

    //     return products;
    // } catch (error) {
    //     console.error("Error on Skapiec scraper:", error);
    //     return [];
    // } finally {
    //     await browser.close();
    // }
}