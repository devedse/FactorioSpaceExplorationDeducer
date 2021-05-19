using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorioSpaceExplorationDeducer
{
    public class CombinationIterator : IEnumerable<int[]>
    {
        private int setSize;
        private int combinationSize;
        private int[] indices;
        private int indexToIncrement;

        /// <summary>
        /// Public constructor takes total set size and number of elements in the combinations to be returned
        /// </summary>
        /// <param name="combinationSize_"></param>
        public CombinationIterator(int combinationSize_, int setSize_)
        {
            if (combinationSize_ <= 0) throw new ArgumentException(string.Format("{0} ({1}) is <= 0", nameof(combinationSize_), combinationSize_));
            if (setSize_ <= 0) throw new ArgumentException(string.Format("{0} ({1}) is <= 0", nameof(setSize_), setSize_));
            if (combinationSize_ > setSize_) throw new ArgumentException(string.Format("{0} ({1}) is <= {2} ({3})",
                nameof(combinationSize_), combinationSize_, nameof(setSize_), setSize_));
            this.combinationSize = combinationSize_;
            this.setSize = setSize_;
            // Create internal list of indices with one additional element to make comparison easier during iteration
            indices = new int[combinationSize + 1];
            for (int i = 0; i < combinationSize; i++)
            {
                indices[i] = i;
            }
            indices[combinationSize] = setSize;
        }

        /// <summary>
        /// Returns subsequent arrays of <see cref="combinationSize"/> indices to enumerate all unique
        /// combinations of elements taken from a set of size <see cref="setSize"/>.<br/>
        /// Based on https://en.wikipedia.org/wiki/Combination "simpler faster way"<br/>
        /// There are many ways to enumerate k combinations. One way is to visit all the binary numbers less than 2n. 
        /// Choose those numbers having k nonzero bits, although this is very inefficient even for small n 
        /// (e.g. n = 20 would require visiting about one million numbers while the maximum number of allowed k combinations 
        /// is about 186 thousand for k = 10). The positions of these 1 bits in such a number is a specific k-combination of 
        /// the set { 1, …, n }.[8] Another simple, faster way is to track k index numbers of the elements selected, 
        /// starting with {0 .. k−1} (zero-based) or {1 .. k} (one-based) as the first allowed k-combination and then 
        /// repeatedly moving to the next allowed k-combination by incrementing the last index number if it is lower 
        /// than n-1 (zero-based) or n (one-based) or the last index number x that is less than the index number 
        /// following it minus one if such an index exists and resetting the index numbers after x to {x+1, x+2, …}.
        /// </summary>
        /// <returns>Subsequently returns an array of <see cref="combinationSize"/> int's such that each element
        /// is the index of an element in the original set, until all such unique combinations have been enumerated.</returns>

        public IEnumerator<int[]> GetEnumerator()
        {
            // On first call the array is already set up, can return it directly
            int[] result = new int[combinationSize];
            Array.Copy(indices, result, combinationSize);
            yield return result;

            indexToIncrement = combinationSize - 1;

            while (indexToIncrement >= 0)
            {
                // Increment at this index until done there
                while (indices[indexToIncrement] < (indices[indexToIncrement + 1] - 1))
                {
                    indices[indexToIncrement]++;
                    result = new int[combinationSize];
                    Array.Copy(indices, result, combinationSize);
                    yield return result;
                }
                // Now start at next lower index
                indexToIncrement--;

                // Are we done?
                if (indexToIncrement < 0) yield break;

                // Room for further iterations here?
                if (indices[indexToIncrement] < (setSize - combinationSize + indexToIncrement))
                {
                    indices[indexToIncrement]++;
                    for (int i = indexToIncrement + 1; i < combinationSize; i++)
                    {
                        indices[i] = indices[i - 1] + 1;
                    }
                    result = new int[combinationSize];
                    Array.Copy(indices, result, combinationSize);
                    yield return result;

                    // If there is more room at the top, we need to start there again
                    if (indices[combinationSize - 1] < (setSize - 1))
                    {
                        indexToIncrement = combinationSize - 1;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
