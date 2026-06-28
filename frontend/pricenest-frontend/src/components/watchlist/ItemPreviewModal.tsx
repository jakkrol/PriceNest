import { WatchlistItem } from "@/types/data";


interface modalProps {
    isOpen: boolean;
    onClose: () => void;
    item: WatchlistItem | null;
}
export default function ItemPreviewModal({ isOpen, onClose, item }: modalProps) {

    if (!isOpen && !item) return null;
    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/70 backdrop-blur-sm">
            <div className="absolute inset-0" onClick={onClose} />
            <div className="relative w-full max-h-[90vh] m-5 rounded-xl border border-white/10 bg-[#121214] p-6 shadow-2xl space-y-6 text-white overflow-y-auto">
                {item?.productName}
            </div>
        </div>
    )
}