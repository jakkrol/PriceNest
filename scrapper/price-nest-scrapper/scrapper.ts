import { chromium } from "playwright";

export default async function scrapeCeneo(productName: string) {
    const browser = await chromium.launch({headless: false});
    const context = await browser.newContext({
        userAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
    });
    const page = await context.newPage();

    try{
        console.log(`Scraping Ceneo for product: ${productName}`);
        await page.goto(`https://www.ceneo.pl/;szukaj-${encodeURIComponent(productName)}`);

        const products = await page.$$eval('.cat-prod-row', (items) => {
            return items.map(item => {
                const title = item.querySelector('.cat-prod-row__name')?.textContent?.trim();
                const price = item.querySelector('.price')?.textContent?.trim();
                return { title, price };
            }).filter(p => p.title && p.price);
        });

        console.table(products);

    } catch (error) {
        console.error("❌ Awaria układu:", error);
    } finally {
        // await browser.close(); // Zakomentuj, żeby widzieć co się stało na stronie
        console.log("🏁 Pomiar zakończony.");
    }


}

