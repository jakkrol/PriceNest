

interface modalProps {
    isOpen: boolean;
}
export default function ItemPreviewModal({ isOpen }: modalProps) {

    if (!isOpen) return null;
    return (
        <div>
            TEST
        </div>
    )
}