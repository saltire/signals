using System;
using System.Collections.Generic;

public static class MIDI {
  public static Dictionary<int, double> notes = new Dictionary<int, double> {
    { 0, 8.1757989156d },
    { 1, 8.6619572180d },
    { 2, 9.1770239974d },
    { 3, 9.7227182413d },
    { 4, 10.3008611535d },
    { 5, 10.9133822323d },
    { 6, 11.5623257097d },
    { 7, 12.2498573744d },
    { 8, 12.9782717994d },
    { 9, 13.7500000000d },
    { 10, 14.5676175474d },
    { 11, 15.4338531643d },
    { 12, 16.3515978313d },
    { 13, 17.3239144361d },
    { 14, 18.3540479948d },
    { 15, 19.4454364826d },
    { 16, 20.6017223071d },
    { 17, 21.8267644646d },
    { 18, 23.1246514195d },
    { 19, 24.4997147489d },
    { 20, 25.9565435987d },
    { 21, 27.5000000000d },
    { 22, 29.1352350949d },
    { 23, 30.8677063285d },
    { 24, 32.7031956626d },
    { 25, 34.6478288721d },
    { 26, 36.7080959897d },
    { 27, 38.8908729653d },
    { 28, 41.2034446141d },
    { 29, 43.6535289291d },
    { 30, 46.2493028390d },
    { 31, 48.9994294977d },
    { 32, 51.9130871975d },
    { 33, 55.0000000000d },
    { 34, 58.2704701898d },
    { 35, 61.7354126570d },
    { 36, 65.4063913251d },
    { 37, 69.2956577442d },
    { 38, 73.4161919794d },
    { 39, 77.7817459305d },
    { 40, 82.4068892282d },
    { 41, 87.3070578583d },
    { 42, 92.4986056779d },
    { 43, 97.9988589954d },
    { 44, 103.8261743950d },
    { 45, 110.0000000000d },
    { 46, 116.5409403795d },
    { 47, 123.4708253140d },
    { 48, 130.8127826503d },
    { 49, 138.5913154884d },
    { 50, 146.8323839587d },
    { 51, 155.5634918610d },
    { 52, 164.8137784564d },
    { 53, 174.6141157165d },
    { 54, 184.9972113558d },
    { 55, 195.9977179909d },
    { 56, 207.6523487900d },
    { 57, 220.0000000000d },
    { 58, 233.0818807590d },
    { 59, 246.9416506281d },
    { 60, 261.6255653006d },
    { 61, 277.1826309769d },
    { 62, 293.6647679174d },
    { 63, 311.1269837221d },
    { 64, 329.6275569129d },
    { 65, 349.2282314330d },
    { 66, 369.9944227116d },
    { 67, 391.9954359817d },
    { 68, 415.3046975799d },
    { 69, 440.0000000000d },
    { 70, 466.1637615181d },
    { 71, 493.8833012561d },
    { 72, 523.2511306012d },
    { 73, 554.3652619537d },
    { 74, 587.3295358348d },
    { 75, 622.2539674442d },
    { 76, 659.2551138257d },
    { 77, 698.4564628660d },
    { 78, 739.9888454233d },
    { 79, 783.9908719635d },
    { 80, 830.6093951599d },
    { 81, 880.0000000000d },
    { 82, 932.3275230362d },
    { 83, 987.7666025122d },
    { 84, 1046.5022612024d },
    { 85, 1108.7305239075d },
    { 86, 1174.6590716696d },
    { 87, 1244.5079348883d },
    { 88, 1318.5102276515d },
    { 89, 1396.9129257320d },
    { 90, 1479.9776908465d },
    { 91, 1567.9817439270d },
    { 92, 1661.2187903198d },
    { 93, 1760.0000000000d },
    { 94, 1864.6550460724d },
    { 95, 1975.5332050245d },
    { 96, 2093.0045224048d },
    { 97, 2217.4610478150d },
    { 98, 2349.3181433393d },
    { 99, 2489.0158697766d },
    { 100, 2637.0204553030d },
    { 101, 2793.8258514640d },
    { 102, 2959.9553816931d },
    { 103, 3135.9634878540d },
    { 104, 3322.4375806396d },
    { 105, 3520.0000000000d },
    { 106, 3729.3100921447d },
    { 107, 3951.0664100490d },
    { 108, 4186.0090448096d },
    { 109, 4434.9220956300d },
    { 110, 4698.6362866785d },
    { 111, 4978.0317395533d },
    { 112, 5274.0409106059d },
    { 113, 5587.6517029281d },
    { 114, 5919.9107633862d },
    { 115, 6271.9269757080d },
    { 116, 6644.8751612791d },
    { 117, 7040.0000000000d },
    { 118, 7458.6201842894d },
    { 119, 7902.1328200980d },
    { 120, 8372.0180896192d },
    { 121, 8869.8441912599d },
    { 122, 9397.2725733570d },
    { 123, 9956.0634791066d },
    { 124, 10548.0818212118d },
    { 125, 11175.3034058561d },
    { 126, 11839.8215267723d },
    { 127, 12543.8539514160d },
  };

  // public static double GetFrequency(int note) {
  //   int a = 440;
  //   return (a / 32) * Math.Pow(2, ((note - 9) / 12));
  // }
}
