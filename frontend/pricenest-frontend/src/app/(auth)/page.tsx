import { ThemeToggle } from "@/components/ThemeToggle";
import { LoginForm } from "@/components/auth/LoginForm";

export default function Home() {
  return (
    <div className="flex flex-col flex-1 items-center justify-center font-sans relative pt-[15vh]">
        <div className="absolute top-6 right-6">
          <ThemeToggle />
        </div>

        <LoginForm />
   
    </div>
  );
}
