import { chromium } from "playwright";

export default async function scrapeCeneo(productName: string) {
    const browser = await chromium.launch({ headless: true });
    const context = await browser.newContext({
        userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
    });
    const page = await context.newPage();

    try {
        productName = productName.toUpperCase()
        console.log(`Scraping Ceneo for product: ${productName}`);
        await page.goto(`https://www.ceneo.pl/;szukaj-${encodeURIComponent(productName)}`, { waitUntil: "domcontentloaded" });

        try {
            await page.waitForSelector('.cat-prod-row__body', { timeout: 5000 });
        } catch (e) {
            console.log("Nie znaleziono standardowych wierszy produktów.");
        }

        await page.mouse.wheel(0, 1000)
        await page.waitForTimeout(500)

        const products = await page.$$eval('.cat-prod-row__body, .cat-prod-row', (items) => {
            console.log("Founded")
            return items.map(item => {
                const rawTitle = item.querySelector('.cat-prod-row__name, .cat-prod-row__desc')?.textContent || "";
                //const title = item.querySelector('.cat-prod-row__content')?.textContent?.trim();
                const rawPrice = item.querySelector('.price')?.textContent || "";
                const cleanTitle = rawTitle.replace(/\s+/g, ' ').trim();

                // Dla ceny usuwamy wszystko poza cyframi, przecinkiem i spacją przed walutą
                const cleanPrice = rawPrice.replace(/\s+/g, ' ').trim();

                return { title: cleanTitle, price: cleanPrice };
            }).filter(p => p.title && p.price);
        });

        //console.table(products);
        // JSON.stringify(JSON.parse(products))
        return products;

    } catch (error) {
        console.error("Error occurred:", error);
    } finally {
        console.log("Success");
        //await page.pause()
        await browser.close();
    }


}

