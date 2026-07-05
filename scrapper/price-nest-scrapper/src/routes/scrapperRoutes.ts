// ../routes/routes.ts
import { Router, type Request, type Response } from "express";
import scrapeAllStores from "../services/scrapper.js"; // Importujesz główny orkiestrator
import { scrapeDailyWatchlist } from "../services/scrapper.js";

const router = Router();

router.post("/scrape", async (req: Request, res: Response) => {
    try {
        const { item } = req.body;

        if (!item) {
            return res.status(400).json({ message: "Item is required" });
        }

        const data = await scrapeAllStores(item);

        return res.json(data);
    } catch (error) {
        console.error("[Router Error]:", error);
        return res.status(500).json({ error: "An error occurred while scraping data" });
    }
});

router.post("/scrape-watchlist", async (req: Request, res: Response) => {
    try {
        const { products } = req.body;
        console.log("[Router] Received products for scraping:", products);
        if (!products || !Array.isArray(products)) {
            return res.status(400).json({ message: "Products array is required" });
        }

        const data = await scrapeDailyWatchlist(products);
        return res.json(data);
    } catch (error) {
        console.error("[Router Error]:", error);
        return res.status(500).json({ error: "An error occurred while scraping watchlist data" });
    }
});

export default router;