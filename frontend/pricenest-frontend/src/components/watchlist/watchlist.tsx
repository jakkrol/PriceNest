'use client'

import { use, useEffect, useState } from "react"
import { axiosGetWatchlist } from "@/api/axios"

export default function Watchlist() {

    const [watchlist, setWatchlist] = useState([])

    useEffect(() => {
        const fetchWatchlist = async () => {
            var res = await axiosGetWatchlist()
            setWatchlist(res.data)
        }

        fetchWatchlist()
    }, [])


    //test data
    useEffect(() => {
        console.log("Watchlist data:", watchlist)
    }, [watchlist])
    return (
        <div className="flex flex-col items-center min-h-screen py-2">
            <h1 className="text-4xl font-bold mb-8 font-serif">Watchlist</h1>


        </div>
    )
}