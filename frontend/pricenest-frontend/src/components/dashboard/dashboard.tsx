'use client'

import { useState } from "react";
import { axiosScrapeProduct } from "@/api/axios";
import AddProductModal from "./AddProductModal";
import { title } from "process";

interface ScrapedProduct {
    id?: string;
    title: string;
    price: string;
    productUrl?: string;
    imageUrl?: string;
    storeName?: string;
}

export default function Dashboard() {
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [selectedProduct, setSelectedProduct] = useState<ScrapedProduct | null>(null);

    const [query, setQuery] = useState<string>("");
    const [isScraping, setIsScraping] = useState<boolean>(false);
    const [scrapedProducts, setScrapedProducts] = useState<ScrapedProduct[]>([]);


    const formatDisplayPrice = (priceStr: string) => {
        if (!priceStr) return "0.00";
        const cleanPrice = priceStr.replace(/\s+/g, '').replace(',', '.');
        const num = parseFloat(cleanPrice);
        return isNaN(num) ? priceStr : num.toFixed(2);
    };

    const handleModalOpen = (product: ScrapedProduct) => {
        setIsModalOpen(true);
        setSelectedProduct(product);
    }

    const handleSearchAndScrape = async (e: React.SubmitEvent) => {
        e.preventDefault();
        if (!query.trim()) return;

        setIsScraping(true);
        try {
            const response = await axiosScrapeProduct(query);
            console.log("Otrzymane dane ze scrapera:", response.data);
            setScrapedProducts(response.data);
        } catch (error) {
            console.error("Błąd podczas uruchamiania scrapera:", error);
        } finally {
            setIsScraping(false);
        }
    };

    // const handleAddToWatchlist = async (product: ScrapedProduct) => {
    //     console.log("Dodaję do watchlisty:", product);
    // };

    return (
        <div className="p-6 space-y-8 min-h-screen text-color">

            {/* === SEKCOJA NAGŁÓWKA I SEARCHBARU === */}
            <div className="max-w-2xl mx-auto text-center space-y-4 mt-8">
                <h1 className="text-3xl font-bold tracking-tight text-color">PriceNest Live Search</h1>
                <p className="text-sm text-gray-400">Wpisz nazwę produktu, aby uruchomić scraper i przeszukać rynek w czasie rzeczywistym.</p>

                <form onSubmit={handleSearchAndScrape} className="flex gap-2 mt-4">
                    <div className="relative w-full">
                        <input
                            type="text"
                            placeholder="Czego szukasz? (np. Laptop Apple MacBook...)"
                            value={query}
                            onChange={(e) => setQuery(e.target.value)}
                            disabled={isScraping}
                            className="block w-full rounded-md bg-white/5 px-4 py-2.5 text-base text-white outline-1 -outline-offset-1 outline-white/10 placeholder:text-gray-500 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6 disabled:opacity-50"
                        />
                    </div>
                    <button
                        type="submit"
                        disabled={isScraping}
                        className="flex justify-center rounded-md bg-indigo-500 px-6 py-2.5 text-sm font-semibold text-white hover:bg-indigo-400 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500 disabled:bg-indigo-500/50 whitespace-nowrap items-center gap-2"
                    >
                        {isScraping ? (
                            <>
                                <span className="animate-spin inline-block h-4 w-4 border-2 border-white border-t-transparent rounded-full"></span>
                                Scrapuję...
                            </>
                        ) : (
                            "Szukaj"
                        )}
                    </button>
                </form>
            </div>

            {/* === SEKCOJA WYNIKÓW SCRAPERU === */}
            <div className="max-w-7xl mx-auto pt-4">

                {/* Stan 1: Ładowanie */}
                {isScraping && (
                    <div className="flex flex-col items-center justify-center py-20 space-y-3">
                        <div className="h-10 w-10 border-4 border-indigo-500 border-t-transparent rounded-full animate-spin"></div>
                        <p className="text-sm font-medium text-gray-400 animate-pulse">
                            Uruchamiam roboty sieciowe... To może potrwać kilkanaście sekund.
                        </p>
                    </div>
                )}

                {/* Stan 2: Ekran startowy */}
                {!isScraping && scrapedProducts.length === 0 && (
                    <div className="text-center py-24 border border-dashed border-white/10 rounded-xl bg-white/5 max-w-xl mx-auto">
                        <h3 className="text-sm font-semibold text-color">Miejsce na Twoje wyniki</h3>
                        <p className="text-xs text-gray-500 mt-1 max-w-xs mx-auto">
                            Po wpisaniu frazy i kliknięciu "Szukaj", tutaj pojawią się najtańsze aktualne oferty z sieci.
                        </p>
                    </div>
                )}

                {/* Stan 3: Wyniki */}
                {!isScraping && scrapedProducts.length > 0 && (
                    <div className="space-y-4">
                        <div className="flex justify-between items-center border-b border-white/10 pb-2">
                            <h2 className="text-lg font-bold text-color">Wyniki wyszukiwania żywego rynku ({scrapedProducts.length})</h2>
                            <button
                                onClick={() => setScrapedProducts([])}
                                className="text-xs text-gray-500 hover:text-gray-300 transition-colors"
                            >
                                Wyczyść wyniki
                            </button>
                        </div>

                        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
                            {scrapedProducts.map((product, index) => (
                                <div key={index} className="rounded-xl border border-white/10 bg-white/5 p-4 flex flex-col justify-between group">
                                    <div>
                                        <div className="h-40 w-full bg-white/5 rounded-lg overflow-hidden flex items-center justify-center p-2 border border-white/5 text-gray-500 text-xs">
                                            {product.imageUrl ? (
                                                <img src={product.imageUrl} alt={product.title} className="max-h-full max-w-full object-contain group-hover:scale-105 transition-transform duration-300" />
                                            ) : (
                                                <span>Brak podglądu</span>
                                            )}
                                        </div>

                                        <div className="mt-3">
                                            <span className="text-[10px] font-bold tracking-wider uppercase bg-white/10 text-gray-300 px-2 py-0.5 rounded">
                                                {product.storeName || "Ceneo"}
                                            </span>
                                        </div>

                                        <h3 className="text-sm font-semibold text-color mt-2 line-clamp-3">
                                            <a href={product.productUrl || "#"} target="_blank" rel="noopener noreferrer" className="hover:text-indigo-400 transition-colors">
                                                {product.title}
                                            </a>
                                        </h3>
                                    </div>

                                    <div className="mt-5 flex justify-between items-center pt-3 border-t border-white/5">
                                        <div className="flex flex-col">
                                            <span className="text-[10px] text-gray-500 font-medium">Cena</span>
                                            <span className="text-lg font-bold text-color">
                                                {formatDisplayPrice(product.price)} zł
                                            </span>
                                        </div>

                                        <button
                                            onClick={() => handleModalOpen(product)}
                                            className="rounded-md bg-indigo-500 px-3 py-1.5 text-xs font-semibold text-white hover:bg-indigo-400 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500 transition-colors"
                                        >
                                            Śledź cenę
                                        </button>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                )}
            </div>

            <AddProductModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} title={selectedProduct?.title || ""} price={selectedProduct?.price || ""} />
        </div>
    );
}