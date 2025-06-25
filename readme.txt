
Readme: Erläuterung der Abgabe

Diese Abgabe beinhaltet die Implementierung eines VR-Lokomotionssystems für ein Skihang-Szenario.

Die zentrale Komponente ist das von uns entwickelte SkiLocomotion-Skript, das es dem Spieler ermöglicht, sich durch realistische Armbewegungen (ähnlich dem Anschieben mit Skistöcken) fortzubewegen.
Das Skript wertet die Relativbewegung der VR-Controller aus und übersetzt diese in Geschwindigkeit entlang der Blickrichtung des Spielers.

---

Designentscheidung und Interface-Erklärung
Unser Interface basiert auf einer körperbasierten Fortbewegung durch Handbewegungen nach hinten, um die Immersion zu steigern und die Interaktion natürlicher zu gestalten.

Die Vorwärtsbewegung ist an die Blickrichtung des Headsets gekoppelt.
Dies erlaubt dem Nutzer, die Bewegungsrichtung intuitiv durch Drehen des Kopfes zu steuern und sorgt für ein natürliches, flüssiges Fahrerlebnis.

---

Verwendete Steuerungs- und Bewegungs-Transferfunktionen
Unser Interface basiert auf zwei unterschiedlichen Transferfunktionen:
eine für die Steuerung und eine für die Geschwindigkeit.

Steuerungs-Transferfunktion (Richtungsbestimmung)
- Wir verwenden eine direkte, lineare Transferfunktion.
- Die Bewegungsrichtung wird aus dem Vorwärtsvektor des Headsets ermittelt und auf die XZ-Ebene projiziert.
- Die Blickrichtung des Nutzers wird 1:1 in die Bewegungsrichtung übertragen.
- Vorteil: Sofortiges, intuitives Steuerungserlebnis ohne Verzögerung oder Pufferung.

Geschwindigkeits-Transferfunktion (Armantrieb)
- Die Geschwindigkeitserhöhung durch Armbewegungen folgt einer quadratischen Transferfunktion.
- Die Bewegung der Controller entlang der lokalen Z-Achse wird gemessen, mit einem Schwellenwert verglichen und quadratisch skaliert.
- Kleine Handbewegungen führen nur zu minimalen Geschwindigkeitsänderungen.
- Kräftige Armstöße erzeugen überproportional mehr Geschwindigkeit.
- Diese nichtlineare Skalierung sorgt für realistisches Feedback, ähnlich wie beim echten Skilanglauf.
- Nachteil: Die Geschwindigkeit steigt bei starken Bewegungen sehr schnell an, was für ungeübte Nutzer schwer kontrollierbar sein könnte.
Eine mögliche Alternative wäre eine logarithmische oder sanft exponentielle Transferfunktion, um große Bewegungen zu dämpfen.

---

Stärken
- Hohe Immersion: Die körperliche Armbewegung passt thematisch und unterstützt das Skigefühl.
- Intuitive Steuerung: Die Blickrichtung steuert die Bewegungsrichtung – schnell verständlich, auch für Anfänger.
- Motion-Sickness-Reduzierung: Durch die körperliche Aktivität wird die Diskrepanz zwischen visueller und physischer Wahrnehmung reduziert.

---

Schwächen
- Ermüdung: Wiederholte Armbewegungen können bei längerer Nutzung anstrengend sein.
- Bewegungserkennung begrenzt: Sehr feine oder langsame Armbewegungen werden eventuell nicht ausreichend erkannt, was zu ungenauer Steuerung führen kann.
- Hohe Geschwindigkeit: Starke Bewegungen erzeugen schnell hohe Geschwindigkeit, was anfangs schwer kontrollierbar sein kann.

---

Nutzbarkeit und Performance-Messung
