using System.Collections.Generic;

public interface ISignalNode {
  float GetValue(double sample, Stack<ISignalNode> nodes);
  float[] GetValues(double sample, int count, Stack<ISignalNode> nodes);
}
