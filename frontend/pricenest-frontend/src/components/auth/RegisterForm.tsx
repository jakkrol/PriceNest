import { axiosRegister } from "@/api/axios";

export function RegisterForm() {

    const handleRegister = async () => {
        try {
            const res = await axiosRegister("user", "123456", "test@example.com");
            console.log(res.data);
        } catch (error) {
            console.error("Error during registration:", error);
        }
    }

    return (
        <div>
            <button onClick={handleRegister} className="mt-4 p-2 bg-green-500 text-white rounded">Register</button>
        </div>
    )
}