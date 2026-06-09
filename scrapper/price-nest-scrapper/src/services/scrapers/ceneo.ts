import { chromium } from "playwright";

export default async function scrapeCeneo(productName: string) {
    const browser = await chromium.launch({ headless: true });
    const context = await browser.newContext({
        userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
    });
    const page = await context.newPage();

    try {
        productName = productName.toUpperCase();
        console.log(`Scraping Ceneo for product: ${productName}`);
        await page.goto(`https://www.ceneo.pl/;szukaj-${encodeURIComponent(productName)}`, { waitUntil: "domcontentloaded" });

        try {
            await page.waitForSelector('.cat-prod-row__body', { timeout: 5000 });
        } catch (e) {
            console.log("Nie znaleziono standardowych wierszy produktów.");
        }

        // Scroll down to trigger lazy loading of images
        await page.mouse.wheel(0, 1000);
        await page.waitForTimeout(1000);

        const products = await page.$$eval('.cat-prod-row', (items) => {
            console.log("Found items to parse");
            return items.map(item => {
                const rawTitle = item.querySelector('.cat-prod-row__name, .cat-prod-row__desc')?.textContent || "";
                const rawPrice = item.querySelector('.price')?.textContent || "";

                // 1. Extract Image URL
                const imgElement = item.querySelector('.cat-prod-row__img img, .js_image-wrapper img, img') as HTMLImageElement | null;
                let imgUrl = "";
                if (imgElement) {
                    imgUrl = imgElement.getAttribute('data-original') ||
                        imgElement.getAttribute('data-src') ||
                        imgElement.src ||
                        "";

                    if (imgUrl.startsWith('//')) {
                        imgUrl = `https:${imgUrl}`;
                    }
                }

                // 2. Extract Product URL
                const linkElement = item.querySelector('a.js_clickHash, a.js_seoUrl, .cat-prod-row__name a, a') as HTMLAnchorElement | null;
                let productUrl = "";
                if (linkElement) {
                    // .href returns the absolute URL directly in the browser context
                    productUrl = linkElement.href || linkElement.getAttribute('href') || "";

                    // Fallback string fix if it somehow grabbed a relative path
                    if (productUrl && productUrl.startsWith('/')) {
                        productUrl = `https://www.ceneo.pl${productUrl}`;
                    }
                }

                const cleanTitle = rawTitle.replace(/\s+/g, ' ').trim();
                const cleanPrice = rawPrice.replace(/\s+/g, ' ').trim();

                return {
                    title: cleanTitle,
                    price: cleanPrice,
                    imageUrl: imgUrl,
                    productUrl: productUrl
                };
            }).filter(p => p.title && p.price);
        });

        return products;

    } catch (error) {
        console.error("Error occurred:", error);
    } finally {
        console.log("Finished scraping process");
        await browser.close();
    }
}