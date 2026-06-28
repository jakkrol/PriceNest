'use client'

import { useEffect, useState } from "react"
import { axiosGetWatchlist } from "@/api/axios"
import { WatchlistItem } from "@/app/types/data"
import ItemPreviewModal from "./ItemPreviewModal"

export default function Watchlist() {

    const [watchlist, setWatchlist] = useState<WatchlistItem[]>([])
    const [isItemDisplayed, setIsItemDisplayed] = useState<boolean>(false)

    useEffect(() => {
        const fetchWatchlist = async () => {
            var res = await axiosGetWatchlist()
            setWatchlist(res.data)
        }

        fetchWatchlist()
    }, [])


    //test data
    useEffect(() => {
        console.log("Watchlist data:", watchlist[0])

    }, [watchlist])

    const handleShowItem = () => {
        setIsItemDisplayed(true)
    }

    //testing output, need to add modal popup or new page on click to check more detailed info and look and some functions or smth, idk
    return (
        <div className="p-4">
            <h1 className="text-2xl font-bold mb-4">Twoja Watchlista</h1>

            <div className="space-y-4">
                {watchlist.map((item) => (
                    <div key={item.productId} className="border p-4 rounded-lg shadow">
                        <h2 className="text-xl font-semibold">{item.productId}</h2>
                        <h2 className="text-xl font-semibold">{item.productName}</h2>
                        <p className="text-gray-600">Cena docelowa: {item.targetPrice} zł</p>
                        <div className="mt-2">
                            <h3 className="font-medium">Dostępne oferty ({item.offers.length}):</h3>
                            {item.offers.map((offer, index) => (
                                <p key={index} className="text-sm text-green-600">
                                    {offer.storeName} - {offer.price} zł
                                </p>
                            ))}
                        </div>
                        <button className="bg-blue-400" onClick={handleShowItem}>Show Data</button>
                    </div>
                ))}
            </div>
            <ItemPreviewModal isOpen={isItemDisplayed} />
        </div>
    );
}