// ../services/scrapper.ts
import scrapeCeneo from "./scrapers/ceneo.js";
import scrapeSkapiec from "./scrapers/skapiec.js";
import scrapeAmazon from "./scrapers/amazon.js";

export default async function scrapeAllStores(productName: string) {
    console.log(`[Mikroserwis] Uruchamianie równoległego wyszukiwania dla: ${productName}`);

    // Odpalamy wszystkie scrapery jednocześnie
    const results = await Promise.allSettled([
        scrapeCeneo(productName),
        scrapeSkapiec(productName),
        scrapeAmazon(productName)
    ]);

    const allProducts: any[] = [];

    results.forEach((result) => {
        if (result.status === "fulfilled" && result.value) {
            // Sklepy zwróciły tablice produktów, wrzucamy je do jednej wspólnej listy
            allProducts.push(...result.value);
        } else if (result.status === "rejected") {
            console.error("[Mikroserwis] Jeden ze scraperów zgłosił błąd:", result.reason);
        }
    });

    // Globalne sortowanie ofert od najtańszej dla całego mikroserwisu
    return allProducts.sort((a, b) => {
        const priceA = parseFloat(a.price.replace(/[^\d,]/g, '').replace(',', '.'));
        const priceB = parseFloat(b.price.replace(/[^\d,]/g, '').replace(',', '.'));
        return (isNaN(priceA) ? Infinity : priceA) - (isNaN(priceB) ? Infinity : priceB);
    });
}