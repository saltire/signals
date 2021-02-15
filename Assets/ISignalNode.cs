using System.Collections.Generic;

public interface ISignalNode {
  float GetValue(double sampleCount, Stack<ISignalNode> nodes);
}
