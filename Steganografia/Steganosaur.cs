namespace Steganografia
{
    public static class Steganosaur
    {
        // Kodowanie tekstu
        public static Bitmap Encode(Bitmap bmp, string text)
        {
            // Wartości subpikseli - składowych kolorów
            int R = 0;
            int G = 0;
            int B = 0;

            int chVal = 0; // Wartość pojedynczego znaku
            int chIdx = 0; // Indeks ukrywanego znaku ze stringa 'text'
            int zeros = 0; // Zlicza zera - potrzebne jako wskaźnik zakończenia kodowania
            int bytePos = 0; // Śledzi aktualną ilość zakodowanych bitów

            bool ending = false; // Flaga pomocnicza zakończenia bajtu

            // Przelatujemy przez wysokość i szerokość obrazka
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    // Zbieramy kolor piksela. Następnie zerujemy najmniej znaczący bit (LSB) każdej jego składowej.
                    Color pxl = bmp.GetPixel(x, y);
                    R = pxl.R - pxl.R % 2;
                    G = pxl.G - pxl.G % 2;
                    B = pxl.B - pxl.B % 2;

                    // Iterujemy przez 3 składowe (R, G, B)...
                    for(int i = 0; i < 3; i++)
                    {
                        if(bytePos % 8 == 0) // Sprawdzamy, czy cały bajt został zakodowany
                        {
                            // Sprawdzamy zakończenie kodowania - jeśli po drodze
                            // zliczonych zostało kolejno 8 zer w kolejnych składowych i zakodowany
                            // został cały bajt...
                            if(zeros == 8 && ending == true)
                            {
                                // ...to sprawiamy by zakodowany został nawet niepełny bajt składowej.
                                // 3 bajty piksela RGB dają łącznie 24 bity, a pojedynczy znak waży 1 bajt (8 bitów).
                                // Oznacza to, że potrzeba ponad 2.5 piksela (a dokładnie 8 z 9 bajtów) na ukrycie w nim zaledwie
                                // jednego znaku - w każdym subpikselu kodowany jest JEDEN BIT znaku.
                                if((bytePos - 1) % 3 < 2)
                                    bmp.SetPixel(x, y, Color.FromArgb(R, G, B));
                                return bmp; // Na koniec zwracamy obrazek ze zmodyfikowaną zawartością i ukrytym tekstem
                            }

                            // Sprawdzamy czy licznik znaków jest przynajmniej równy długości tekstu
                            // i ustawiamy flagę zakończenia kodowania.
                            // Jeśli nie, to pobieramy kolejny znak.
                            if(chIdx >= text.Length)
                                ending = true;
                            else
                                chVal = text[chIdx++];
                        }

                        // Tu się odbywa właściwe kodowanie...
                        switch(bytePos % 3)
                        {
                            case 0:
                                if(!ending)
                                {
                                    // W każdym subpikselu zwyczajnie dodajemy 1 do LSB,
                                    // a potem usuwany ten zakodowany bit w znaku.
                                    R += chVal % 2;
                                    chVal /= 2;
                                }
                                break;

                            case 1:
                                if(!ending)
                                {
                                    G += chVal % 2;
                                    chVal /= 2;
                                }
                                break;

                            case 2:
                                if(!ending)
                                {
                                    B += chVal % 2;
                                    chVal /= 2;
                                }
                                bmp.SetPixel(x, y, Color.FromArgb(R, G, B)); // Nadpisujemy piksel.
                                break;
                        }

                        // Inkrementujemy liczniki
                        bytePos++;
                        if(ending)
                            zeros++;
                    }
                }
            }
            return bmp;
        }

        // Odkodowanie
        public static string Decode(Bitmap bmp)
        {
            string str = string.Empty; // Zmienna przechowująca rozkodowany tekst
            int chVal = 0;  // Wartość pojedynczego znaku
            int bytePos = 0; // Licznik pozycji w bajcie

            // Lecimy przez wysokość i szerokość obrazka...
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    Color col = bmp.GetPixel(x, y); // Odczytujemy kolor
                    
                    // Iterujemy po składowych...
                    for(int subPx = 0; subPx < 3; subPx++)
                    {
                        switch(bytePos % 3)
                        {
                            case 0:
                                // Bierzemy LSB z subpiksela i dodajemy do kolejnych
                                // bitów w znaku. Warto zauważych, że robi się to od prawej do lewej,
                                // Więc na koniec cały bajt trzeba przerzucić - zamienić bity miejscami - MSB na LSB.
                                chVal = chVal * 2 + col.R % 2;
                                break;

                            case 1:
                                chVal = chVal * 2 + col.G % 2;
                                break;

                            case 2:
                                chVal = chVal * 2 + col.B % 2;
                                break;
                        }
                        bytePos++;

                        // Gdy cały bajt zostanie rozkodowany...
                        if(bytePos % 8 == 0)
                        {
                            chVal = ReverseBits(chVal); // ...zamieniamy miejscami bity...
                            if(chVal == 0)
                                return str;
                            str += (char)chVal; // ...i składamy do wyjściowego stringa
                        }
                    }
                }
            }
            return str; // Zwracamy odkodowany tekst
        }

        // Metoda pomocnicza do odwracania kolejności bitów
        private static int ReverseBits(int n)
        {
            int res = 0;
            for(int i = 0; i < 8; i++)
            {
                res = res * 2 + n % 2;
                n /= 2;
            }
            return res;
        }
    }
}
