"use client";

import { axiosAddToWatchlist } from "@/api/axios";
import { useEffect, useState } from "react";

interface AddProductModalProps {
    isOpen: boolean;
    onClose: () => void;
    title: string;
    productUrl: string;
    price: string;
}


export default function AddProductModal({ isOpen, onClose, title, productUrl, price }: AddProductModalProps) {

    const [selectedWords, setSelectedWords] = useState<string[]>([]);
    var titleAr = title.split(" ");
    const stopWords = new Set([
        "for",
        "with",
        "and",
        "the",
        "of",
        "to",
        "in",
        "on",
        "at",
        "a",
        "an",
        "&",
        "free",
        "day",
    ]);
    const keywords = titleAr.map(word => word.toLowerCase().replace(/^[,.;:!?()]+|[,.;:!?()]+$/g, "")).filter(word => word.length > 1).filter(word => !stopWords.has(word))

    // const toggleSelect = (word: string) => {
    //     if (selectedWords.includes(word)) {
    //         // Jesli wybrane - usun
    //         setSelectedWords(selectedWords.filter((w) => w !== word));
    //     } else {
    //         // Jesli nie wybrane - dodaj
    //         setSelectedWords([...selectedWords, word]);
    //     }

    // };

    const toggleSelect = (word: string) => {
        if (selectedWords.includes(word)) {
            setSelectedWords(selectedWords.filter((w) => w !== word));
        } else {
            setSelectedWords([...selectedWords, word]);
        }
    }

    const handleSelect = async () => {
        alert(selectedWords)
        try {
            // testing api call with custom prices
            var fullName = selectedWords.join(" ")
            await axiosAddToWatchlist(fullName, productUrl, 2000, 1000, "")
        } catch (error) {
            alert("Error occurred while adding product to watchlist. Please try again.");
        }
    }


    if (!isOpen) return null;
    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/70 backdrop-blur-sm">
            <div className="absolute inset-0" onClick={onClose} />

            <div className="relative w-full max-w-md rounded-xl border border-white/10 bg-[#121214] p-6 shadow-2xl space-y-6 text-white">
                <div className="flex justify-between items-center">
                    <h3 className="text-lg font-bold">Szablon Modala</h3>
                    <button
                        onClick={onClose}
                        className="absolute top-4 right-4 text-gray-500 hover:text-white transition-colors p-1 text-lg"
                    >
                        ✕
                    </button>
                </div>

                <div className="py-4 border-y border-white/5 text-sm text-gray-400">
                    {/* TODO - map the title and wrap them into separate buttons to check out and create a new normalized product name based on the selected parts */}
                    {/* {titleAr[0]} */}
                    {/* Kontener na pigułki (Flexbox z zawijaniem) */}
                    <div className="flex flex-wrap gap-2">
                        {keywords.map((word, id) => {
                            const isSelected = selectedWords.includes(word);

                            return (
                                <button
                                    key={id}
                                    onClick={() => toggleSelect(word)}
                                    type="button"
                                    className={`
                                                px-4 py-1.5 text-sm font-medium rounded-full transition-all duration-200 border
                                                ${isSelected
                                            ? 'bg-indigo-600 text-white border-indigo-600 shadow-sm scale-95'
                                            : 'bg-gray-100 text-gray-700 border-gray-200 hover:bg-gray-200 hover:border-gray-300'
                                        }
                                    `}
                                >
                                    {word}
                                </button>
                            );
                        })}
                    </div>
                </div>

                <div className="flex justify-end gap-3">
                    <button
                        type="button"
                        onClick={onClose}
                        className="rounded-md border border-white/10 px-4 py-2 text-xs font-semibold text-gray-400 hover:text-white hover:bg-white/5 transition-colors"
                    >
                        Anuluj
                    </button>
                    <button
                        type="button"
                        onClick={handleSelect}
                        className="rounded-md bg-indigo-500 px-4 py-2 text-xs font-semibold text-white hover:bg-indigo-400 transition-colors"
                    >
                        Uruchom monitoring
                    </button>
                </div>
            </div>
        </div>
    );
}
