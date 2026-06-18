using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.common
{
    public static class Messages
    {
        private static readonly Dictionary<string, string> en = new()
        {
            ["welcome.title"] = "WELCOME!",
            ["menu.login"] = "1. Login",
            ["menu.register"] = "2. Register",
            ["menu.exit"] = "0. Exit",
            ["error.invalid_selection"] = "Invalid selection. Please choose a valid option.",
            ["error.invalid_number"] = "Invalid input. Please enter a valid number.",
            ["login.success"] = "~Login successful",
            ["login.failed"] = "~Login failed",
            ["register.success"] = "~Register request sent",
            ["register.password_mismatch"] = "Passwords do not match. Please try again.",
            ["paper.not_found"] = "Research not found.",
            ["permission.denied"] = "You do not have permission to perform this action.",
            ["confirm.uploaded"] = "~Paper uploaded",
            ["confirm.deleted"] = "~Paper deleted",
            ["admin.no_requests"] = "~No Request at this point",
            ["prompt.enter_email"] = "Enter your Email",
            ["prompt.enter_password"] = "Enter your Password",
            ["prompt.confirm_password"] = "Confirm your Password",
            ["prompt.enter_title"] = "Enter the Title:",
            ["prompt.enter_abstract"] = "Enter the Abstract:",
            ["info.goodbye"] = "~ Good Bye..",
        };

        private static readonly Dictionary<string, string> id = new()
        {
            ["welcome.title"] = "SELAMAT DATANG!",
            ["menu.login"] = "1. Masuk",
            ["menu.register"] = "2. Daftar",
            ["menu.exit"] = "0. Keluar",
            ["error.invalid_selection"] = "Pilihan tidak valid. Silakan pilih opsi yang benar.",
            ["error.invalid_number"] = "Input tidak valid. Masukkan angka yang benar.",
            ["login.success"] = "~Berhasil masuk",
            ["login.failed"] = "~Login gagal",
            ["register.success"] = "~Permintaan pendaftaran dikirim",
            ["register.password_mismatch"] = "Kata sandi tidak cocok. Coba lagi.",
            ["paper.not_found"] = "Riset tidak ditemukan.",
            ["permission.denied"] = "Anda tidak memiliki izin untuk melakukan tindakan ini.",
            ["confirm.uploaded"] = "~Riset berhasil diunggah",
            ["confirm.deleted"] = "~Riset dihapus",
            ["admin.no_requests"] = "~Tidak ada permintaan saat ini",
            ["prompt.enter_email"] = "Masukkan Email Anda",
            ["prompt.enter_password"] = "Masukkan Kata Sandi Anda",
            ["prompt.confirm_password"] = "Konfirmasi Kata Sandi",
            ["prompt.enter_title"] = "Masukkan Judul:",
            ["prompt.enter_abstract"] = "Masukkan Abstrak:",
            ["info.goodbye"] = "~ Sampai jumpa..",
        };

        public static string Get(string key)
        {
            string lang = Config.Load().Language?.ToLower() ?? "en";
            return lang switch
            {
                "id" or "in" => id.ContainsKey(key) ? id[key] : $"[missing:{key}]",
                _ => en.ContainsKey(key) ? en[key] : $"[missing:{key}]",
            };
        }
    }
}
