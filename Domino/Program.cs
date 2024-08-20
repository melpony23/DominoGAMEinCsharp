internal class Domino
{
    private static void RotarFicha(ref (int, int) ficha)
    {
        ficha = (ficha.Item2, ficha.Item1);
    }

    private static (bool, int) JugandoPC((int, string)[] jugadores, Dictionary<int, List<(int, int)>> fichasPorJugador,
        int turno, int[] turnos, List<(int, int)> mesa)
    {
        var fichasJugador = fichasPorJugador[jugadores[turno - 1].Item1];
        var jugadorTurno = Array.IndexOf(turnos, turno);
        var extremoInicial = mesa.Count > 0 ? mesa[0].Item1 : -1;
        var extremoFinal = mesa.Count > 0 ? mesa[mesa.Count - 1].Item2 : -1;
        var fichasValidas = new List<(int, int)>();
        var pasaTurno = 0;
        foreach (var ficha in fichasJugador)
            if (ficha.Item1 == extremoInicial || ficha.Item2 == extremoInicial ||
                ficha.Item1 == extremoFinal || ficha.Item2 == extremoFinal || (ficha.Item1 == 6 && ficha.Item2 == 6))
                fichasValidas.Add(ficha);

        if (fichasValidas.Count == 0)
        {
            Console.WriteLine($"El jugador {jugadorTurno} pasa su turno");
            Thread.Sleep(2000);
            Console.WriteLine("");
            return (false, 1);
        }

        var rnd = new Random();
        var rndId = rnd.Next(fichasValidas.Count);

        var fichaSelect = fichasValidas[rndId];

        if (fichaSelect.Item1 == extremoFinal)
        {
            mesa.Add(fichaSelect);
            fichasJugador.Remove(fichaSelect);
        }
        else if (fichaSelect.Item2 == extremoFinal)
        {
            fichasJugador.Remove(fichaSelect);
            RotarFicha(ref fichaSelect);
            mesa.Add(fichaSelect);
        }
        else if (fichaSelect.Item1 == extremoInicial)
        {
            fichasJugador.Remove(fichaSelect);
            RotarFicha(ref fichaSelect);
            mesa.Insert(0, fichaSelect);
        }
        else
        {
            mesa.Insert(0, fichaSelect);
            fichasJugador.Remove(fichaSelect);
        }


        Console.WriteLine($"Ficha jugada por el jugador {jugadorTurno}: {fichaSelect}");
        Thread.Sleep(2000);
        Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
        Console.WriteLine("");

        if (fichasJugador.Count == 0)
        {
            Console.WriteLine("");
            Console.WriteLine($"El jugador {jugadorTurno} ha ganado!");
            Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
            return (true, pasaTurno);
        }

        return (false, pasaTurno);
    }

    private static (bool, int) JugandoUser((int, string)[] jugadores,
        Dictionary<int, List<(int, int)>> fichasPorJugador,
        int turno, int[] turnos, List<(int, int)> mesa)
    {
        var fichasJugador = fichasPorJugador[jugadores[turno - 1].Item1];
        var jugadorTurno = Array.IndexOf(turnos, turno);
        var extremoInicial = mesa.Count > 0 ? mesa[0].Item1 : -1;
        var extremoFinal = mesa.Count > 0 ? mesa[mesa.Count - 1].Item2 : -1;
        var fichasValidas = new List<(int, int)>();
        var pasaTurno = 0;
        Console.WriteLine("Tus fichas disponibles son:");
        var countFichas = 0;
        foreach (var ficha in fichasJugador)
        {
            if (ficha.Item1 == extremoInicial || ficha.Item2 == extremoInicial ||
                ficha.Item1 == extremoFinal || ficha.Item2 == extremoFinal || (ficha.Item1 == 6 && ficha.Item2 == 6))
                fichasValidas.Add(ficha);

            Thread.Sleep(500);
            Console.WriteLine($"{countFichas}. [{ficha.Item1}, {ficha.Item2}]");
            countFichas++;
        }

        if (fichasValidas.Count == 0)
        {
            Console.WriteLine("No tienes fichas validas para bajar a la mesa...");
            Thread.Sleep(2000);
            Console.WriteLine($"El jugador {jugadorTurno} pasa su turno");
            Console.WriteLine("");
            return (false, 1);
        }

        Console.WriteLine($"Selecciona la opcion con la ficha que quieres bajar (ej: 0. [6,6] " +
                          $"(ingresa 0 para seleccionar la ficha)");
        var fichaAceptada = false;
        while (fichaAceptada != true)
        {
            var opcion = Console.ReadLine();
            int.TryParse(opcion, out var selection);
            var fichaSelect = fichasJugador[selection];

            var validoInicial = 0;
            var rotarInicial = 0;
            var rotarFinal = 0;
            var validoFinal = 0;

            if (fichaSelect.Item1 == extremoFinal) validoFinal++;

            if (fichaSelect.Item2 == extremoFinal)
            {
                validoFinal++;
                rotarFinal++;
            }

            if (fichaSelect.Item1 == extremoInicial)
            {
                validoInicial++;
                rotarInicial++;
            }

            if (fichaSelect.Item2 == extremoInicial || (fichaSelect.Item1 == 6 && fichaSelect.Item2 == 6))
                validoInicial++;

            if (validoInicial >= 1 && validoFinal >= 1)
            {
                Console.WriteLine("Deseas colocar la ficha al inicio o al final? (ej: inicio)");
                var eleccion = Console.ReadLine();
                if (eleccion == "inicio")
                {
                    if (rotarInicial >= 1)
                    {
                        fichasJugador.Remove(fichaSelect);
                        RotarFicha(ref fichaSelect);
                        mesa.Insert(0, fichaSelect);
                        Console.WriteLine($"Ficha jugada por el jugador {jugadorTurno}: {fichaSelect}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
                        Console.WriteLine("");
                        fichaAceptada = true;
                        break;
                    }
                    else
                    {
                        mesa.Insert(0, fichaSelect);
                        fichaAceptada = true;
                        fichasJugador.Remove(fichaSelect);
                        Console.WriteLine($"Ficha jugada por el jugador {jugadorTurno}: {fichaSelect}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
                        Console.WriteLine("");
                        fichaAceptada = true;
                        break;
                    }
                }

                if (eleccion == "final")
                {
                    if (rotarFinal >= 1)
                    {
                        fichasJugador.Remove(fichaSelect);
                        RotarFicha(ref fichaSelect);
                        mesa.Add(fichaSelect);
                        Console.WriteLine($"Ficha jugada por el jugador {jugadorTurno}: {fichaSelect}");
                        Thread.Sleep(2000);
                        Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
                        Console.WriteLine("");
                        fichaAceptada = true;
                        break;
                    }

                    mesa.Add(fichaSelect);
                    fichaAceptada = true;
                    fichasJugador.Remove(fichaSelect);
                    Console.WriteLine($"Ficha jugada por el jugador {jugadorTurno}: {fichaSelect}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
                    Console.WriteLine("");
                    break;
                }
            }
            else
            {
                if ((validoInicial == 1 && rotarInicial == 0) || (validoInicial == 2 && rotarInicial == 1))
                {
                    mesa.Insert(0, fichaSelect);
                    fichasJugador.Remove(fichaSelect);
                    fichaAceptada = true;
                }
                else if ((validoFinal == 1 && rotarFinal == 0) || (validoFinal == 2 && rotarFinal == 1))
                {
                    mesa.Add(fichaSelect);
                    fichasJugador.Remove(fichaSelect);
                    fichaAceptada = true;
                }
                else if (validoInicial == 1 && rotarInicial == 1)
                {
                    fichasJugador.Remove(fichaSelect);
                    RotarFicha(ref fichaSelect);
                    mesa.Insert(0, fichaSelect);
                    fichaAceptada = true;
                }
                else if (validoFinal == 1 && rotarFinal == 1)
                {
                    fichasJugador.Remove(fichaSelect);
                    RotarFicha(ref fichaSelect);
                    mesa.Add(fichaSelect);
                    fichaAceptada = true;
                }

                if (rotarFinal == 0 && rotarInicial == 0 && validoInicial == 0 && validoFinal == 0)
                {
                    fichaAceptada = false;
                    Thread.Sleep(2000);
                    Console.WriteLine("Seleccion invalida :(");
                    Console.WriteLine("Selecciona otra opcion de ficha");
                }
                else
                {
                    fichaAceptada = true;
                    Console.WriteLine($"Ficha jugada por el jugador {jugadorTurno}: {fichaSelect}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
                    Console.WriteLine("");
                }
            }
        }

        if (fichasJugador.Count == 0)
        {
            Console.WriteLine("");
            Console.WriteLine($"El jugador {jugadorTurno} ha ganado!");
            Console.WriteLine($"Mesa: {string.Join(", ", mesa)}");
            return (true, pasaTurno);
        }

        return (false, pasaTurno);
    }

    private static (int[] Turnos, Dictionary<int, List<(int, int)>> FichasPorJugador) RepartirFichasAndTurnos(
        (int, string)[] jugadores, List<(int, int)> fichas)
    {
        var turnos = new int[4]; // Arreglo de los 4 turnos en orden
        var numeroTurno = 0;
        var fichasTotales = fichas.Count;

        var fichasPorJugador = new Dictionary<int, List<(int, int)>>();

        foreach (var jugador in jugadores) fichasPorJugador[jugador.Item1] = new List<(int, int)>();

        var rnd = new Random();

        foreach (var jugador in jugadores)
            for (var i = 0; i < 7; i++)
            {
                var rndId = rnd.Next(fichasTotales); // Número random de 0 a fichas totales
                var ficha = fichas[rndId];
                fichas.RemoveAt(rndId);
                --fichasTotales;

                if (ficha == (6, 6))
                {
                    turnos[numeroTurno] = jugador.Item1; // Asignamos como primer turno al que tenga el chancho (6,6)
                    numeroTurno++;
                }

                fichasPorJugador[jugador.Item1].Add(ficha);
            }

        // Asignando los 3 turnos restantes 
        for (var i = 0; i < 3; i++)
        {
            var rndId2 = rnd.Next(1, 5); // Número aleatorio entre 1 y 4 (IDs de usuario)

            if (!turnos.Contains(rndId2))
            {
                turnos[numeroTurno] = rndId2;
                numeroTurno++;
            }
            else
            {
                i--;
            }
        }

        return (turnos, fichasPorJugador); // Devuelve los turnos y diccionario
    }

    private static void Main()
    {
        Console.WriteLine("Bienvenido al Domino's game!! :)");
        var fichas = new List<(int, int)>
        {
            (0, 0), (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (0, 6),
            (1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 6),
            (2, 2), (2, 3), (2, 4), (2, 5), (2, 6),
            (3, 3), (3, 4), (3, 5), (3, 6),
            (4, 4), (4, 5), (4, 6),
            (5, 5), (5, 6),
            (6, 6)
        };

        var jugadores = new[]
        {
            (1, "PC"),
            (2, "PC"),
            (3, "PC"),
            (4, "USER")
        };

        Console.WriteLine("Se comienzan a repartir las fichas...");
        Console.WriteLine("********************************************");
        Console.WriteLine("");
        Thread.Sleep(2000);
        var (turnos, fichasPorJugador) = RepartirFichasAndTurnos(jugadores, fichas);
        var ganador = false;
        var pasaTurno = 0;
        var pasaTurnofunc = 0;
        var mesa = new List<(int, int)>();

        while (ganador != true)
            foreach (var turno in turnos)
            {
                if (jugadores[turno - 1].Item2 == "PC")
                {
                    Console.WriteLine($"Juega el jugador {Array.IndexOf(turnos, turno)} (PC)");
                    Thread.Sleep(2000);
                    (ganador, pasaTurnofunc) = JugandoPC(jugadores, fichasPorJugador, turno, turnos, mesa);
                }
                else
                {
                    Console.WriteLine($"Juega el jugador {Array.IndexOf(turnos, turno)} (Usuario)");
                    (ganador, pasaTurnofunc) = JugandoUser(jugadores, fichasPorJugador, turno, turnos, mesa);
                }

                if (ganador) break;

                if (pasaTurnofunc == 1)
                    pasaTurno++;
                else
                    pasaTurno = 0;

                if (pasaTurno == 4)
                {
                    Console.WriteLine("Hay un empate! fin del juego");
                    ganador = true;
                    break;
                }
            }
    }
}