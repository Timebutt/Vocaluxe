#region license
// This file is part of Vocaluxe.
// 
// Vocaluxe is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Vocaluxe is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Vocaluxe. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using NAudio.Midi;

namespace Vocaluxe.Lib.Midi
{
    public sealed class CMidiInterface
    {
        private static readonly CMidiInterface instance = new CMidiInterface();
        private static MidiIn midiInputDevice;
        private static MidiOut midiOutputDevice;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CMidiInterface()
        {
            string searchString = "loopMIDI";

            // Find the loopMIDI input device
            int inputDeviceCount = MidiIn.NumberOfDevices;
            for (int i = 0; i < inputDeviceCount; i++)
            {
                var inputDeviceInfo = MidiIn.DeviceInfo(i);
                if (inputDeviceInfo.ProductName.Contains(searchString))
                {
                    midiInputDevice = new MidiIn(i);
                }
            }

            // Find the loopMIDI output device
            int outputDeviceCount = MidiOut.NumberOfDevices;
            for (int i = 0; i < outputDeviceCount; i++)
            {
                var outputDeviceInfo = MidiOut.DeviceInfo(i);
                if (outputDeviceInfo.ProductName.Contains(searchString))
                {
                    midiOutputDevice = new MidiOut(i);
                }
            }
        }

        private CMidiInterface()
        {
        }

        public static CMidiInterface Instance
        {
            get
            {
                return instance;
            }
        }

        public static MidiIn MidiIn
        {
            get
            {
                return midiInputDevice;
            }
        }

        public static void sendMidiNote(int midiNote, int value = 127)
        {
            midiOutputDevice.Send(MidiMessage.StartNote(midiNote, value, 1).RawData);
            midiOutputDevice.Send(MidiMessage.StopNote(midiNote, value, 1).RawData);
        }

        public static void sendStartNote()
        {
            sendMidiNote(2);
        }

        public static void sendStopNote()
        {
            sendMidiNote(3);
        }

        public static void sendPauseNote()
        {
            sendMidiNote(4);
        }

        public static void sendScore(double score)
        {   
            int firstTwoDigits = (int)Math.Round(score / 100);
            int lastTwoDigits = (int)Math.Round(score) % 100;

            sendMidiNote(10, firstTwoDigits);
            sendMidiNote(11, lastTwoDigits);
        }

        public static void finalizeScore()
        {   
            sendMidiNote(20);
        }
    }
}