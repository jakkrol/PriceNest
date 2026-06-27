export interface ScrapedProduct {
    id?: string;
    title: string;
    price: string;
    productUrl?: string;
    imageUrl?: string;
    storeName?: string;
}

export interface ProductOffer {
    storeName: string;
    price: number;
    url: string;
    lastUpdated: Date;
}

export interface WatchlistItem {
    productId: number;
    productName: string;
    targetPrice: number;
    prefferedStores: string;
    offers: ProductOffer[]
}


