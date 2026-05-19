import { Router, type Request, type Response } from "express";
import scrapeCeneo from "../services/scrapper.js";

const router = Router();

router.post("/scrape", async (req: Request, res: Response) => {
    try {
        const { item } = req.body;

        if (!item) {
            return res.status(400).json({ message: "Item is required" });
        }

        const data = await scrapeCeneo(item);
        res.json(data);
    } catch (error) {
        res.status(500).json({ error: "An error occurred while scraping data" });
    }

});

export default router;