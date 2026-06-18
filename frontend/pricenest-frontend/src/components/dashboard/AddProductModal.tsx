"use client";

import { axiosAddToWatchlist } from "@/api/axios";

interface AddProductModalProps {
    isOpen: boolean;
    onClose: () => void;
    title: string;
    price: string;
}


export default function AddProductModal({ isOpen, onClose, title, price }: AddProductModalProps) {

    var titleAr = title.split(" ");

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
                    {titleAr[0]}
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
                        onClick={() => alert("Zapisano! (Logika do dodania)")}
                        className="rounded-md bg-indigo-500 px-4 py-2 text-xs font-semibold text-white hover:bg-indigo-400 transition-colors"
                    >
                        Uruchom monitoring
                    </button>
                </div>
            </div>
        </div>
    );
}
