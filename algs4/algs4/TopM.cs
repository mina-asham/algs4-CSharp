using algs4.stdlib;

namespace algs4.algs4
{
    public static class TopM
    {
        /// <summary>
        /// Reads a sequence of transactions from standard input; takes a
        /// command-line integer M; prints to standard output the M largest
        /// transactions in descending order.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int m = int.Parse(args[0]);
            MinPQ<Transaction> pq = new MinPQ<Transaction>(m + 1);

            // top M entries are on the PQ
            while (StdIn.HasNextLine())
            {
                // Create an entry from the next line and put on the PQ. 
                string line = StdIn.ReadLine();
                Transaction transaction = new Transaction(line);
                pq.Insert(transaction);

                // remove minimum if M+1 entries on the PQ
                if (pq.Size() > m)
                {
                    pq.DelMin();
                }
            }

            // print entries on PQ in reverse order
            Stack<Transaction> stack = new Stack<Transaction>();
            foreach (Transaction transaction in pq)
            {
                stack.Push(transaction);
                stack.Push(transaction);
            }
            foreach (Transaction transaction in stack)
            {
                StdOut.PrintLn(transaction);
            }
        }
    }
}