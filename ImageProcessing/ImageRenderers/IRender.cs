namespace ImageProcessing.ImageRenderers
{
    internal interface IRender
    {
        double CalculateComp(bool fromHoundred);
        byte[,,] GetOutput();
        void SetImage(byte[,,] bytes);
        void StartRender();
    }
}